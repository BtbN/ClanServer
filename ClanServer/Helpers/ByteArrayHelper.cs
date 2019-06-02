using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ClanServer.Helpers
{
    public static class ByteArrayHelper
    {
        private static readonly uint[] toHexLookup;
        private static unsafe readonly uint* toHexLookupP;
        private static readonly byte[] fromHexLookupLo, fromHexLookupHi;
        private static unsafe readonly byte* fromHexLookupLoP, fromHexLookupHiP;

        static unsafe ByteArrayHelper()
        {
            toHexLookup = new uint[256];
            fromHexLookupLo = new byte[256];
            fromHexLookupHi = new byte[256];

            for (int i = 0; i < 256; ++i)
            {
                string s = i.ToString("X2");
                if (BitConverter.IsLittleEndian)
                    toHexLookup[i] = s[0] | ((uint)s[1] << 16);
                else
                    toHexLookup[i] = s[1] | ((uint)s[0] << 16);

                fromHexLookupLo[i] = fromHexLookupHi[i] = 255;
            }

            for (int i = 0; i < 16; ++i)
            {
                int ii = i | (i << 4);
                string v = ii.ToString("X2");

                fromHexLookupHi[v[0]] = (byte)(i << 4);
                fromHexLookupLo[v[1]] = (byte)i;

                v = v.ToLower();

                fromHexLookupHi[v[0]] = (byte)(i << 4);
                fromHexLookupLo[v[1]] = (byte)i;
            }

            toHexLookupP = (uint*)GCHandle.Alloc(toHexLookup, GCHandleType.Pinned).AddrOfPinnedObject();
            fromHexLookupLoP = (byte*)GCHandle.Alloc(fromHexLookupLo, GCHandleType.Pinned).AddrOfPinnedObject();
            fromHexLookupHiP = (byte*)GCHandle.Alloc(fromHexLookupHi, GCHandleType.Pinned).AddrOfPinnedObject();
        }

        public static unsafe string ToHexString(this byte[] bytes)
        {
            uint* lookupP = toHexLookupP;
            string result = new string(' ', bytes.Length * 2);

            fixed (byte* bytesP = bytes)
            fixed (char* resultP = result)
            {
                uint* resultP2 = (uint*)resultP;

                for (int i = 0; i < bytes.Length; i++)
                {
                    resultP2[i] = lookupP[bytesP[i]];
                }
            }

            return result;
        }

        public static unsafe byte[] ToBytesFromHex(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return new byte[0];
            if ((source.Length & 1) != 0)
                throw new ArgumentException();

            int len = source.Length >> 1;
            byte[] result = new byte[len];

            fixed (char* src = source)
            fixed (byte* res = result)
            {
                for (int i = 0; i < len; ++i)
                {
                    char c = src[i * 2];
                    byte hi, lo;

                    if (c > 255 || (hi = fromHexLookupHiP[c]) == 255)
                        throw new ArgumentException();

                    c = src[i * 2 + 1];

                    if (c > 255 || (lo = fromHexLookupLoP[c]) == 255)
                        throw new ArgumentException();

                    res[i] = (byte)(hi | lo);
                }
            }

            return result;
        }
    }
}
