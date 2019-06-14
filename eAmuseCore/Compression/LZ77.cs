using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eAmuseCore.Compression
{
    public class RepeatingRingBuffer
    {
        private readonly byte[] buf;
        private readonly int mod;
        private int pos;

        public RepeatingRingBuffer(int size)
        {
            if (size == 0 || (size & (size - 1)) != 0)
                throw new ArgumentException("Size is not a power of two.", "size");

            buf = new byte[size];
            mod = size - 1;
            pos = 0;
        }

        public byte this[int index]
        {
            get => buf[(pos + index) & mod];
            set => buf[(pos + index) & mod] = value;
        }

        public int Size
        {
            get => buf.Length;
        }

        public void Append(byte b)
        {
            buf[pos++ & mod] = b;
        }

        public IEnumerable<byte> Slice(int start, int end)
        {
            for (int i = start; i < end; ++i)
            {
                if (start < 0 && i >= 0)
                    yield return this[start + (i % start)];
                else
                    yield return this[i];
            }
        }
    }

    public static class LZ77
    {
        private const int minLength = 3;

        public static IEnumerable<byte> Decompress(IEnumerable<byte> data)
        {
            RepeatingRingBuffer buffer = new RepeatingRingBuffer(0x1000);
            int state = 0;

            using (var iter = data.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    state >>= 1;
                    if (state <= 1)
                    {
                        state = iter.Current | 0x100;
                        if (!iter.MoveNext())
                            yield break;
                    }

                    if ((state & 1) != 0)
                    {
                        yield return iter.Current;
                        buffer.Append(iter.Current);
                    }
                    else
                    {
                        byte byte1 = iter.Current;
                        if (!iter.MoveNext())
                            yield break;
                        byte byte2 = iter.Current;

                        int length = (byte2 & 0xf) + minLength;
                        int distance = (byte1 << 4) | (byte2 >> 4);

                        if (distance == 0)
                            yield break;

                        for (int i = 0; i < length; ++i)
                        {
                            byte b = buffer[-distance];
                            yield return b;
                            buffer.Append(b);
                        }
                    }
                }
            }
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
