using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace eAmuseCore.Compression
{
    class RingBuffer
    {
        private byte[] buf;
        private int pos;
        private int mod;

        public RingBuffer(int size)
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
                yield return this[i];
        }
    }

    public static class LZ77
    {
        private const int minLength = 3;

        public static IEnumerable<byte> Decompress(IEnumerable<byte> data)
        {
            RingBuffer buffer = new RingBuffer(0x1000);
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

        private static Match findLongestMatch(RingBuffer data, int prefetchLength, int windowSize)
        {
            Match res = new Match
            {
                distance = -1,
                length = -1
            };

            for (int j = 2 - prefetchLength; j < 0; ++j)
            {
                var sub = data.Slice(-prefetchLength, j);
                int subLength = j + prefetchLength;
                if (subLength <= res.length)
                    continue;

                for (int i = -prefetchLength - windowSize; i < -prefetchLength; i++)
                {
                    var slice = Enumerable.Empty<byte>();
                    int dist = -prefetchLength - i;

                    var repSlice = data.Slice(i, -prefetchLength);
                    for (int reps = subLength / dist; reps > 0; --reps)
                        slice = slice.Concat(repSlice);

                    var match = slice.Concat(data.Slice(i, i + (subLength % dist)));

                    if (match.ToArray() == sub)
                    {
                        res.distance = dist;
                        res.length = subLength;
                        Console.WriteLine("match: " + res.distance + ":" + res.length);
                        var a = match.ToArray();
                        if (a != sub)
                            Console.WriteLine("FALSE!");
                    }
                }
            }

            return res;
        }

        public static IEnumerable<byte> Compress(IEnumerable<byte> data, int windowSize = 32, int lookAhead = 0xf + minLength)
        {
            if (lookAhead < 3 || lookAhead > 0xf + minLength)
                throw new ArgumentException("lookAhead out of range", "lookAhead");
            if (windowSize < lookAhead)
                throw new ArgumentException("windowSize needs to be larger than lookAhead", "windowSize");
            if ((windowSize & (windowSize - 1)) != 0)
                throw new ArgumentException("windowSize is not a power of two.", "windowSize");

            windowSize = Math.Min(windowSize, 0xfff);
            int prefetchLenght = 0;

            RingBuffer inputQueue = new RingBuffer(windowSize);
            Queue<byte> outputQueue = new Queue<byte>();
            Queue<byte> state = new Queue<byte>();
            bool finished = false;

            using (var iter = data.GetEnumerator())
            {
                while (true)
                {
                    while (prefetchLenght <= lookAhead && iter.MoveNext())
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
                        Match match = findLongestMatch(inputQueue, prefetchLenght, windowSize);
                        if (match.length >= minLength && match.distance > 0)
                        {
                            prefetchLenght -= match.length;
                            match.length -= minLength;

                            if (match.length > 0xf || match.distance > 0xfff)
                                throw new Exception("INTERNAL ERROR: found match is too long!");

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

                        Console.WriteLine("state: " + state.Select((b, i) => b << i).Sum());

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
