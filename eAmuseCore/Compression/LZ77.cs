using System;
using System.Collections.Generic;
using System.Linq;

namespace eAmuseCore.Compression
{
    public static class LZ77
    {
        public static IEnumerable<byte> Decompress(IEnumerable<byte> data)
        {
            byte[] buffer = new byte[0x1000];
            int pos = 0;
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

                    if ((state & 1) == 1)
                    {
                        yield return iter.Current;
                        buffer[pos++ & 0xfff] = iter.Current;
                    }
                    else
                    {
                        byte byte1 = iter.Current;
                        if (!iter.MoveNext())
                            yield break;
                        byte byte2 = iter.Current;

                        int length = (byte2 & 0xf) + 3;
                        int distance = (byte1 << 4) | (byte2 >> 4);

                        if (distance == 0)
                            yield break;

                        for (int i = 0; i < length; ++i)
                        {
                            byte b = buffer[(pos - distance) & 0xfff];
                            yield return b;
                            buffer[pos++ & 0xfff] = b;
                        }
                    }
                }
            }
        }
    }
}
