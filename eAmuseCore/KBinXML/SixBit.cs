using System.Collections.Generic;
using System.Numerics;
using System;

namespace eAmuseCore.KBinXML
{
    public static class SixBit
    {
        static readonly string charmap = "0123456789:ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz";
        static readonly Dictionary<char, byte> bytemap = new Dictionary<char, byte>();

        static SixBit()
        {
            for (byte i = 0; i < charmap.Length; ++i)
                bytemap[charmap[i]] = i;
        }

        public static byte[] Pack(string input)
        {
            if (input.Length > byte.MaxValue)
                throw new ArgumentException("input string is too long", "input");

            int length_bits = input.Length * 6;
            int length_bytes = (length_bits + 7) / 8;
            int padding = (8 - (length_bits % 8)) % 8;

            BigInteger bits = new BigInteger(0);

            try
            {
                foreach (char c in input)
                {
                    bits <<= 6;
                    bits |= bytemap[c];
                }
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("Input string contains invalid sixbit characters.", "input");
            }

            bits <<= padding;

            byte[] res = new byte[length_bytes + 1];
            Buffer.BlockCopy(bits.ToByteArray(), 0, res, 1, length_bytes);
            Array.Reverse(res, 1, length_bytes);
            res[0] = (byte)input.Length;

            return res;
        }

        public static string Unpack(byte[] data)
        {
            return Unpack(new ByteBuffer(data));
        }

        public static string Unpack(ByteBuffer nodeBuf)
        {
            int length = nodeBuf.TakeU8();

            int length_bits = length * 6;
            int length_bytes = (length_bits + 7) / 8;
            int padding = (8 - (length_bits % 8)) % 8;

            byte[] bytes = nodeBuf.TakeBytes(length_bytes);
            Array.Reverse(bytes);
            BigInteger bits = new BigInteger(bytes);

            bits >>= padding;

            char[] res = new char[length];
            for (int i = length - 1; i >= 0; --i)
            {
                int idx = (int)(bits & 0x3f);
                bits >>= 6;
                res[i] = charmap[idx];
            }
            return new string(res);
        }
    }
}
