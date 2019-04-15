using System;
using System.Collections.Generic;
using System.Linq;

namespace eAmuseCore.KBinXML
{
    static class EnumHelpers
    {
        public static uint FirstU32(this IEnumerable<byte> input)
        {
            input = input.Take(4);
            if (BitConverter.IsLittleEndian)
                input = input.Reverse();
            return BitConverter.ToUInt32(input.ToArray(), 0);
        }

        public static int FirstI32(this IEnumerable<byte> input)
        {
            input = input.Take(4);
            if (BitConverter.IsLittleEndian)
                input = input.Reverse();
            return BitConverter.ToInt32(input.ToArray(), 0);
        }

        public static ushort FirstU16(this IEnumerable<byte> input)
        {
            input = input.Take(2);
            if (BitConverter.IsLittleEndian)
                input = input.Reverse();
            return BitConverter.ToUInt16(input.ToArray(), 0);
        }

        public static short FirstI16(this IEnumerable<byte> input)
        {
            input = input.Take(2);
            if (BitConverter.IsLittleEndian)
                input = input.Reverse();
            return BitConverter.ToInt16(input.ToArray(), 0);
        }
    }
}
