using System.Collections.Generic;
using System.Collections;
using System.Numerics;
using System.Text;
using System.Linq;
using System;

namespace eAmuseCore.KBinXML
{
    static class SixBit
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

            int padding = (8 - (input.Length * 6)) % 8;
            BigInteger bits = new BigInteger(0);

            try
            {
                foreach (byte i in input.Select(c => bytemap[c]))
                {
                    bits <<= 6;
                    bits |= i;
                }
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("input string contains invalid sixbit characters.", "input");
            }

            bits <<= padding;

            return bits.ToByteArray().Append((byte)input.Length).Reverse().ToArray();
        }

        public static string Unpack(byte[] data)
        {
            IEnumerable<byte> nodeBuf = data.AsEnumerable();
            return Unpack(ref nodeBuf);
        }

        public static string Unpack(ref IEnumerable<byte> nodeBuf)
        {
            int length = nodeBuf.FirstU8();
            nodeBuf = nodeBuf.Skip(1);

            int length_bits = length * 6;
            int length_bytes = (length_bits + 7) / 8;
            int padding = (8 - (length_bits % 8)) % 8;

            // bytes are in big endian order, BigInteger expects little endian, hence .Reverse() it.
            BigInteger bits = new BigInteger(nodeBuf.TakeU8(length_bytes).Reverse().ToArray());
            nodeBuf = nodeBuf.Skip(length_bytes);
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
