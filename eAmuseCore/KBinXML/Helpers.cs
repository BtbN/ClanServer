using System;

namespace eAmuseCore.KBinXML.Helpers
{
    static class ByteArrayHelpers
    {
        public static ulong GetU64(this byte[] input, int offset = 0)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(input, offset, 8);
            return BitConverter.ToUInt64(input, offset);
        }

        public static long GetS64(this byte[] input, int offset = 0)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(input, offset, 8);
            return BitConverter.ToInt64(input, offset);
        }

        public static uint GetU32(this byte[] input, int offset = 0)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(input, offset, 4);
            return BitConverter.ToUInt32(input, offset);
        }

        public static int GetS32(this byte[] input, int offset = 0)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(input, offset, 4);
            return BitConverter.ToInt32(input, offset);
        }

        public static ushort GetU16(this byte[] input, int offset = 0)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(input, offset, 2);
            return BitConverter.ToUInt16(input, offset);
        }

        public static short GetS16(this byte[] input, int offset = 0)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(input, offset, 2);
            return BitConverter.ToInt16(input, offset);
        }

        public static byte GetU8(this byte[] input, int offset = 0)
        {
            return input[offset];
        }

        public static sbyte GetS8(this byte[] input, int offset = 0)
        {
            return unchecked((sbyte)input[offset]);
        }

        public static float GetFloat(this byte[] input, int offset = 0)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(input, offset, 4);
            return BitConverter.ToSingle(input, offset);
        }

        public static double GetDouble(this byte[] input, int offset = 0)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(input, offset, 8);
            return BitConverter.ToDouble(input, offset);
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
