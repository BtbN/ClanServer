using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Linq;
using System;

namespace eAmuseCore.KBinXML
{
    static class SixBit
    {
        static readonly string charmap = "0123456789:ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz";

        public static string Unpack(ref IEnumerable<byte> nodeBuf)
        {
            int length = nodeBuf.First();
            nodeBuf = nodeBuf.Skip(1);

            int length_bits = length * 6;
            int length_bytes = (length_bits + 7) / 8;

            BitArray barr = new BitArray(nodeBuf.Take(length_bytes).ToArray());
            nodeBuf = nodeBuf.Skip(length_bytes);

            StringBuilder res = new StringBuilder();
            for (int i = 0, pos = length_bits - 1; i < length; ++i)
            {
                int resI = 0;
                for (int j = 0; j < 6; ++j, --pos)
                    if (barr.Get(pos))
                        resI |= 1 << j;

                res.Append(charmap[resI]);
            }
            return res.ToString();
        }
    }
}
