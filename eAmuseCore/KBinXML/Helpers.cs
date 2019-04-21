using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eAmuseCore.KBinXML.Helpers
{
    static class EnumHelpers
    {
        public static IEnumerable<ulong> TakeU64(this IEnumerable<byte> input, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                input = input.Take(8);
                if (BitConverter.IsLittleEndian)
                    input = input.Reverse();
                yield return BitConverter.ToUInt64(input.ToArray(), 0);
            }
        }

        public static IEnumerable<long> TakeS64(this IEnumerable<byte> input, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                input = input.Take(8);
                if (BitConverter.IsLittleEndian)
                    input = input.Reverse();
                yield return BitConverter.ToInt64(input.ToArray(), 0);
            }
        }

        public static IEnumerable<uint> TakeU32(this IEnumerable<byte> input, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                input = input.Take(4);
                if (BitConverter.IsLittleEndian)
                    input = input.Reverse();
                yield return BitConverter.ToUInt32(input.ToArray(), 0);
            }
        }

        public static IEnumerable<int> TakeS32(this IEnumerable<byte> input, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                input = input.Take(4);
                if (BitConverter.IsLittleEndian)
                    input = input.Reverse();
                yield return BitConverter.ToInt32(input.ToArray(), 0);
            }
        }

        public static IEnumerable<ushort> TakeU16(this IEnumerable<byte> input, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                input = input.Take(2);
                if (BitConverter.IsLittleEndian)
                    input = input.Reverse();
                yield return BitConverter.ToUInt16(input.ToArray(), 0);
            }
        }

        public static IEnumerable<short> TakeS16(this IEnumerable<byte> input, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                input = input.Take(2);
                if (BitConverter.IsLittleEndian)
                    input = input.Reverse();
                yield return BitConverter.ToInt16(input.ToArray(), 0);
            }
        }

        public static IEnumerable<byte> TakeU8(this IEnumerable<byte> input, int count)
        {
            return input.Take(count);
        }

        public static IEnumerable<sbyte> TakeS8(this IEnumerable<byte> input, int count)
        {
            return input.Take(count).Select(b => unchecked((sbyte)b));
        }

        public static IEnumerable<float> TakeF(this IEnumerable<byte> input, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                input = input.Take(4);
                if (BitConverter.IsLittleEndian)
                    input = input.Reverse();
                yield return BitConverter.ToSingle(input.ToArray(), 0);
            }
        }

        public static IEnumerable<double> TakeD(this IEnumerable<byte> input, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                input = input.Take(8);
                if (BitConverter.IsLittleEndian)
                    input = input.Reverse();
                yield return BitConverter.ToDouble(input.ToArray(), 0);
            }
        }

        public static ulong FirstU64(this IEnumerable<byte> input)
        {
            return input.TakeU64(1).First();
        }

        public static long FirstS64(this IEnumerable<byte> input)
        {
            return input.TakeS64(1).First();
        }

        public static uint FirstU32(this IEnumerable<byte> input)
        {
            return input.TakeU32(1).First();
        }

        public static int FirstS32(this IEnumerable<byte> input)
        {
            return input.TakeS32(1).First();
        }

        public static ushort FirstU16(this IEnumerable<byte> input)
        {
            return input.TakeU16(1).First();
        }

        public static short FirstS16(this IEnumerable<byte> input)
        {
            return input.TakeS16(1).First();
        }

        public static byte FirstU8(this IEnumerable<byte> input)
        {
            return input.TakeU8(1).First();
        }

        public static sbyte FirstS8(this IEnumerable<byte> input)
        {
            return input.TakeS8(1).First();
        }

        public static float FirstF(this IEnumerable<byte> input)
        {
            return input.TakeF(1).First();
        }

        public static double FirstD(this IEnumerable<byte> input)
        {
            return input.TakeD(1).First();
        }

        public static void AddU8(this List<byte> list, byte data)
        {
            list.Add(data);
        }

        public static void AddS8(this List<byte> list, sbyte data)
        {
            list.Add(unchecked((byte)data));
        }

        public static void AddRangeAligned(this List<byte> list, IEnumerable<byte> data, int alignment = 4)
        {
            list.AddRange(data);
            list.Realign(alignment);
        }

        public static IEnumerable<byte> TakeBytesAligned(ref IEnumerable<byte> input, int size, int alignment = 4)
        {
            var res = input.Take(size);
            input = input.Skip(size);

            int align = alignment - (size % alignment);
            if (align != alignment)
                input = input.Skip(align);

            return res;
        }

        public static string TakeStringAligned(ref IEnumerable<byte> input, Encoding encoding, int alignment = 4)
        {
            int size = input.FirstS32();
            input = input.Skip(4);
            byte[] data = TakeBytesAligned(ref input, size, alignment).ToArray();
            return encoding.GetString(data, 0, data.Length - 1); // drop final null byte
        }
    }

    public static class ListHelpers
    {
        public static void AddU32(this List<byte> list, uint data)
        {
            IEnumerable<byte> bytes = BitConverter.GetBytes(data);
            if (BitConverter.IsLittleEndian)
                bytes = bytes.Reverse();
            list.AddRange(bytes);
        }

        public static void AddS32(this List<byte> list, int data)
        {
            IEnumerable<byte> bytes = BitConverter.GetBytes(data);
            if (BitConverter.IsLittleEndian)
                bytes = bytes.Reverse();
            list.AddRange(bytes);
        }

        public static void Realign(this List<byte> list, int alignment = 4)
        {
            int align = alignment - (list.Count % alignment);
            if (align == alignment)
                return;
            while (align-- > 0)
                list.Add(0);
        }
    }

    public static class PrimHelpers
    {
        public static byte[] ToBytes(this byte i)
        {
            return new byte[] { i };
        }

        public static byte[] ToBytes(this sbyte i)
        {
            return new byte[] { unchecked((byte)i) };
        }

        public static byte[] ToBytes(this short i)
        {
            byte[] res = BitConverter.GetBytes(i);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(res);
            return res;
        }

        public static byte[] ToBytes(this ushort i)
        {
            byte[] res = BitConverter.GetBytes(i);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(res);
            return res;
        }

        public static byte[] ToBytes(this int i)
        {
            byte[] res = BitConverter.GetBytes(i);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(res);
            return res;
        }

        public static byte[] ToBytes(this uint i)
        {
            byte[] res = BitConverter.GetBytes(i);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(res);
            return res;
        }

        public static byte[] ToBytes(this long i)
        {
            byte[] res = BitConverter.GetBytes(i);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(res);
            return res;
        }

        public static byte[] ToBytes(this ulong i)
        {
            byte[] res = BitConverter.GetBytes(i);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(res);
            return res;
        }

        public static byte[] ToBytes(this float i)
        {
            byte[] res = BitConverter.GetBytes(i);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(res);
            return res;
        }

        public static byte[] ToBytes(this double i)
        {
            byte[] res = BitConverter.GetBytes(i);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(res);
            return res;
        }

        public static bool ToBool(this string s)
        {
            try
            {
                return Convert.ToBoolean(s);
            }
            catch(FormatException)
            {
                try
                {
                    return Convert.ToBoolean(Convert.ToInt32(s));
                }
                catch(FormatException)
                {
                    return false;
                }
            }
        }
    }
}
