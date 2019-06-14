using System;
using System.Collections.Generic;

namespace eAmuseCore.Compression
{
    public static class LZ77
    {
        private const int minLength = 3;

        public static byte[] Decompress(byte[] data)
        {
            List<byte> res = new List<byte>();

            int pos = 0;
            int state = 0;

            while (pos < data.Length)
            {
                state >>= 1;
                if (state <= 1)
                    state = data[pos++] | 0x100;

                if ((state & 1) != 0)
                {
                    res.Add(data[pos++]);
                }
                else
                {
                    byte byte1 = data[pos++];
                    byte byte2 = data[pos++];

                    int length = (byte2 & 0xf) + minLength;
                    int distance = (byte1 << 4) | (byte2 >> 4);

                    if (distance == 0)
                        break;

                    int resPos = res.Count;
                    for (int i = 0; i < length; ++i)
                    {
                        int o = resPos - distance + i;
                        res.Add((o < 0) ? (byte)0 : res[o]);
                    }
                }
            }

            return res.ToArray();
        }

        private struct Match
        {
            public int distance;
            public int length;
        }

        private static Match FindLongestMatch(byte[] data, int offset, int windowSize, int lookAhead, int minLength)
        {
            Match res = new Match
            {
                distance = -1,
                length = -1
            };

            int maxLength = Math.Min(lookAhead, data.Length - offset);

            for (int matchLength = maxLength; matchLength >= minLength; --matchLength)
            {
                for (int distance = 1; distance <= windowSize; ++distance)
                {
                    if (data.RepeatingSequencesEqual(offset, matchLength, offset - distance, distance))
                    {
                        res.distance = distance;
                        res.length = matchLength;
                        return res;
                    }
                }
            }

            return res;
        }

        private static bool RepeatingSequencesEqual(this byte[] arr, int matchOffset, int matchLength, int compOffset, int compLength)
        {
            for (int i = 0; i < matchLength; ++i)
            {
                if (arr.GV(matchOffset + i) != arr.GV(compOffset + (i % compLength)))
                    return false;
            }

            return true;
        }

        private static byte GV(this byte[] arr, int i)
        {
            return (i < 0) ? (byte)0 : arr[i];
        }

        public static byte[] Compress(byte[] data, int windowSize = 256, int lookAhead = 0xf + minLength)
        {
            if (lookAhead < minLength || lookAhead > 0xf + minLength)
                throw new ArgumentException("lookAhead out of range", "lookAhead");
            if (windowSize < lookAhead)
                throw new ArgumentException("windowSize needs to be larger than lookAhead", "windowSize");

            byte[] result = new byte[data.Length + (data.Length / 8) + 4];
            int resOffset = 1;
            int resStateOffset = 0;
            int resStateShift = 0;
            int offset = 0;

            while (offset < data.Length)
            {
                Match match = FindLongestMatch(data, offset, windowSize, lookAhead, minLength);
                if (match.length >= minLength && match.distance > 0)
                {
                    int binLength = match.length - minLength;

#if DEBUG
                    if (binLength > 0xf || match.distance > 0xfff || match.length > data.Length - offset)
                        throw new Exception("INTERNAL ERROR: found match is invalid!");
#endif

                    result[resOffset++] = (byte)(match.distance >> 4);
                    result[resOffset++] = (byte)(((match.distance & 0xf) << 4) | binLength);
                    resStateShift += 1;
                    offset += match.length;
                }
                else
                {
                    result[resStateOffset] |= (byte)(1 << resStateShift++);
                    result[resOffset++] = data[offset++];
                }

                if (resStateShift >= 8)
                {
                    resStateShift = 0;
                    resStateOffset = resOffset++;
                }
            }

            Array.Resize(ref result, resOffset + 2);

            return result;
        }
    }
}
