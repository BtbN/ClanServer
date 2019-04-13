using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System;

namespace eAmuseCore.Crypto
{
    public static class RC4
    {
        private static readonly byte[] konamiCode = 
            new byte[]{ 0x69, 0xd7, 0x46, 0x27, 0xd9, 0x85, 0xee, 0x21, 0x87, 0x16, 0x15, 0x70, 0xd0,
                        0x8d, 0x93, 0xb1, 0x24, 0x55, 0x03, 0x5b, 0x6d, 0xf0, 0xd8, 0x20, 0x5d, 0xf5 };

        public static IEnumerable<byte> ApplyEAmuseInfo(string info, IEnumerable<byte> data)
        {
            if (!info.StartsWith("1-") || info.Count(c => c == '-') != 2 || info.Length != 15)
                throw new ArgumentException("Unknown E-Amuse-Info format.", "info");
            info = info.Substring(2).Replace("-", "");
            byte[] key = Enumerable.Range(0, info.Length / 2).Select(i => Convert.ToByte(info.Substring(i * 2, 2), 16)).ToArray();
            return ApplyEAmuse(key, data);
        }

        public static IEnumerable<byte> ApplyEAmuse(byte[] key, IEnumerable<byte> data)
        {
            if (key.Length != 6)
                throw new ArgumentException("Key length has to be exactly 6 bytes.", "key");

            using (MD5 md5 = MD5.Create())
            {
                byte[] realKey = md5.ComputeHash(key.Concat(konamiCode).ToArray());
                return EncryptOutput(realKey, data);
            }
        }

        public static IEnumerable<byte> Apply(byte[] key, IEnumerable<byte> data)
        {
            return EncryptOutput(key, data);
        }

        private static byte[] EncryptInitalize(byte[] key)
        {
            byte[] s = Enumerable.Range(0, 0x100).Select(i => (byte)i).ToArray();

            for (uint i = 0, j = 0; i < 0x100; i++)
            {
                j = (j + key[i % key.Length] + s[i]) & 0xff;
                Swap(s, i, j);
            }

            return s;
        }

        private static IEnumerable<byte> EncryptOutput(byte[] key, IEnumerable<byte> data)
        {
            byte[] s = EncryptInitalize(key);

            uint i = 0;
            uint j = 0;

            foreach (byte b in data)
            {
                i = (i + 1) & 0xff;
                j = (j + s[i]) & 0xff;

                Swap(s, i, j);

                yield return (byte)(b ^ s[(s[i] + s[j]) & 0xff]);
            };
        }

        private static void Swap(byte[] s, uint i, uint j)
        {
            byte c = s[i];

            s[i] = s[j];
            s[j] = c;
        }
    }
}
