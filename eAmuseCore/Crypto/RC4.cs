using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System;

namespace eAmuseCore.Crypto
{
    public static class RC4
    {
        private static readonly byte[] konamiCode = 
            new byte[]{ 105, 215, 70, 39, 217, 133, 238, 33, 135,
                        22, 21, 112, 208, 141, 147, 177, 36, 85,
                        3, 91, 109, 240, 216, 32, 93, 245 };

        public static IEnumerable<byte> ApplyEAmuseInfo(string info, IEnumerable<byte> data)
        {
            if (!info.StartsWith("1-") || info.Count(c => c == '-') != 3 || info.Length != 15)
                throw new ArgumentException("Unknown E-Amuse-Info format.", "info");
            info = info.Substring(2).Replace("-", "");
            byte[] key = Enumerable.Range(0, info.Length / 2).Select(i => Convert.ToByte(info.Substring(i * 2, 2))).ToArray();
            return ApplyEAmuse(key, data);
        }

        public static IEnumerable<byte> ApplyEAmuse(byte[] key, IEnumerable<byte> data)
        {
            if (key.Length != 6)
                throw new ArgumentException("Key length has to be exactly 6 bytes.", "key");

            using (MD5 md5 = MD5.Create())
            {
                byte[] realKey = md5.ComputeHash(key.Concat(konamiCode).ToArray());
                return Apply(realKey, data);
            }
        }

        public static IEnumerable<byte> Apply(byte[] key, IEnumerable<byte> data)
        {
            return EncryptOutput(key, data);
        }

        private static byte[] EncryptInitalize(byte[] key)
        {
            byte[] s = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();

            for (uint i = 0, j = 0; i < 256; i++)
            {
                j = (j + key[i % key.Length] + s[i]) & 255;
                Swap(s, i, j);
            }

            return s;
        }

        private static IEnumerable<byte> EncryptOutput(byte[] key, IEnumerable<byte> data)
        {
            byte[] s = EncryptInitalize(key);

            uint i = 0;
            uint j = 0;

            return data.Select(b =>
            {
                i = (i + 1) & 255;
                j = (j + s[i]) & 255;

                Swap(s, i, j);

                return (byte)(b ^ s[(s[i] + s[j]) & 255]);
            });
        }

        private static void Swap(byte[] s, uint i, uint j)
        {
            byte c = s[i];

            s[i] = s[j];
            s[j] = c;
        }
    }
}
