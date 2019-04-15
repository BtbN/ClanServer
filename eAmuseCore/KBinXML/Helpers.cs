using System;
using System.Collections.Generic;
using System.Linq;

namespace eAmuseCore.KBinXML
{
    static class EnumHelpers
    {
        public static IEnumerable<ulong> TakeU64(this IEnumerable<byte> input, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                var state = input.Take(8);
                if (BitConverter.IsLittleEndian)
                    state = state.Reverse();
                input = input.Skip(8);
                yield return BitConverter.ToUInt64(state.ToArray(), 0);
            }
        }

        public static IEnumerable<long> TakeS64(this IEnumerable<byte> input, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                var state = input.Take(8);
                if (BitConverter.IsLittleEndian)
                    state = state.Reverse();
                input = input.Skip(8);
                yield return BitConverter.ToInt64(state.ToArray(), 0);
            }
        }

        public static IEnumerable<uint> TakeU32(this IEnumerable<byte> input, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                var state = input.Take(4);
                if (BitConverter.IsLittleEndian)
                    state = state.Reverse();
                input = input.Skip(4);
                yield return BitConverter.ToUInt32(state.ToArray(), 0);
            }
        }

        public static IEnumerable<int> TakeS32(this IEnumerable<byte> input, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                var state = input.Take(4);
                if (BitConverter.IsLittleEndian)
                    state = state.Reverse();
                input = input.Skip(4);
                yield return BitConverter.ToInt32(state.ToArray(), 0);
            }
        }

        public static IEnumerable<ushort> TakeU16(this IEnumerable<byte> input, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                var state = input.Take(2);
                if (BitConverter.IsLittleEndian)
                    state = state.Reverse();
                input = input.Skip(2);
                yield return BitConverter.ToUInt16(state.ToArray(), 0);
            }
        }

        public static IEnumerable<short> TakeS16(this IEnumerable<byte> input, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                var state = input.Take(2);
                if (BitConverter.IsLittleEndian)
                    state = state.Reverse();
                input = input.Skip(2);
                yield return BitConverter.ToInt16(state.ToArray(), 0);
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
                var state = input.Take(4);
                if (BitConverter.IsLittleEndian)
                    state = state.Reverse();
                input = input.Skip(4);
                yield return BitConverter.ToSingle(state.ToArray(), 0);
            }
        }

        public static IEnumerable<double> TakeD(this IEnumerable<byte> input, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                var state = input.Take(8);
                if (BitConverter.IsLittleEndian)
                    state = state.Reverse();
                input = input.Skip(8);
                yield return BitConverter.ToDouble(state.ToArray(), 0);
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

        public static IEnumerable<object> Box<T>(this IEnumerable<T> input)
        {
            return input.Select(v => (object)v);
        }
    }
}
