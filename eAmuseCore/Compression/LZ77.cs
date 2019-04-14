using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eAmuseCore.Compression
{
    public class RepeatingRingBuffer
    {
        private byte[] buf;
        private int pos;
        private int mod;

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

        private static Match findLongestMatch(RepeatingRingBuffer data, int prefetchLength, int windowSize, int minLength)
        {
            Match res = new Match
            {
                distance = -1,
                length = -1
            };

            minLength = Math.Min(1, minLength);

            for (int j = 0; j >= minLength - prefetchLength; --j)
            {
                var sub = data.Slice(-prefetchLength, j);
                int subLength = j + prefetchLength;

                for (int distance = 1; distance < windowSize - prefetchLength; ++distance)
                {
                    int start = -prefetchLength - distance;
                    var match = data.Slice(start, start + subLength);

                    if (match.SequenceEqual(sub))
                    {
                        res.distance = distance;
                        res.length = subLength;
                        return res;
                    }
                }
            }

            return res;
        }

        public static IEnumerable<byte> Compress(IEnumerable<byte> data, int windowSize = 256, int lookAhead = 0xf + minLength)
        {
            if (lookAhead < 3 || lookAhead > 0xf + minLength)
                throw new ArgumentException("lookAhead out of range", "lookAhead");
            if (windowSize < lookAhead)
                throw new ArgumentException("windowSize needs to be larger than lookAhead", "windowSize");
            if ((windowSize & (windowSize - 1)) != 0)
                throw new ArgumentException("windowSize is not a power of two.", "windowSize");

            windowSize = Math.Min(windowSize, 0x1000);
            int prefetchLenght = 0;

            RepeatingRingBuffer inputQueue = new RepeatingRingBuffer(windowSize);
            Queue<byte> outputQueue = new Queue<byte>();
            Queue<byte> state = new Queue<byte>();
            bool finished = false;

            using (var iter = data.GetEnumerator())
            {
                while (true)
                {
                    while (prefetchLenght < lookAhead && iter.MoveNext())
                    {
                        inputQueue.Append(iter.Current);
                        prefetchLenght += 1;
                    }

                    if (prefetchLenght <= 0)
                    {
                        state.Enqueue(0);
                        outputQueue.Enqueue(0);
                        outputQueue.Enqueue(0);
                        finished = true;
                    }
                    else
                    {
                        Match match = findLongestMatch(inputQueue, prefetchLenght, windowSize, minLength);
                        if (match.length >= minLength && match.distance > 0)
                        {
                            prefetchLenght -= match.length;
                            match.length -= minLength;

                            if (match.length > 0xf || match.distance > 0xfff || prefetchLenght < 0)
                                throw new Exception("INTERNAL ERROR: found match is invalid!");

                            state.Enqueue(0);
                            outputQueue.Enqueue((byte)(match.distance >> 4));
                            outputQueue.Enqueue((byte)(((match.distance & 0xf) << 4) | match.length));
                        }
                        else
                        {
                            state.Enqueue(1);
                            outputQueue.Enqueue(inputQueue[-prefetchLenght]);
                            prefetchLenght -= 1;
                        }
                    }

                    if (state.Count == 8 || finished)
                    {
                        yield return (byte)state.Select((b, i) => b << i).Sum();
                        foreach (byte b in outputQueue)
                            yield return b;

                        state.Clear();
                        outputQueue.Clear();
                    }

                    if (finished)
                        yield break;
                }
            }
        }
    }
}
