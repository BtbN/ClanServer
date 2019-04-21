using System;
using System.Collections.Generic;
using System.Text;

namespace eAmuseCore.KBinXML
{
    public class ByteBuffer
    {
        private List<byte> data;
        private int wordWriteOffset = 0, byteWriteOffset = 0;
        private int wordReadOffset = 0, byteReadOffset = 0;
        private int limit = -1;
        private readonly int align = 4;

        public int Offset { get; set; } = 0;

        public int Length
        {
            get => (limit >= 0) ? limit : data.Count;
        }

        public int Remaining
        {
            get => Length - Offset;
        }

        public bool AtEnd
        {
            get => Offset >= Length;
        }

        public ByteBuffer()
        {
            data = new List<byte>();
        }

        public ByteBuffer(IEnumerable<byte> data)
        {
            this.data = new List<byte>(data);
        }

        public ByteBuffer(ByteBuffer other)
        {
            data = other.data;
            Offset = other.Offset;
        }

        public byte this[int idx]
        {
            get => data[idx];
            set => data[idx] = value;
        }

        public void CopyTo(int idx, byte[] arr, int offset, int count)
        {
            data.CopyTo(idx, arr, offset, count);
        }

        public ByteBuffer MakeSub(int offset, int length = -1)
        {
            ByteBuffer res = new ByteBuffer(this);
            res.Offset += offset;
            if (length >= 0 && offset + length <= Length)
                res.limit = offset + length;
            return res;
        }

        public void RealignReads()
        {
            int realign = align - (Offset % align);
            if (realign == align)
                return;
            Offset += realign;
        }

        public void RealignWrites()
        {
            int realign = align - (data.Count % align);
            if (realign == align)
                return;
            while (realign-- > 0)
                data.Add(0);
        }

        public byte[] TakeBytes(int count)
        {
            byte[] res = new byte[count];
            data.CopyTo(Offset, res, 0, count);
            Offset += count;
            return res;
        }

        public byte[] TakeBytesSubAligned(int count)
        {
            byte[] res = new byte[count];

            if (count == 1)
            {
                if (byteReadOffset % align == 0)
                {
                    byteReadOffset = Offset;
                    Offset += align;
                }
                data.CopyTo(byteReadOffset, res, 0, 1);
                byteReadOffset += 1;
            }
            else if (count == 2)
            {
                if (wordReadOffset % align == 0)
                {
                    wordReadOffset = Offset;
                    Offset += align;
                }
                data.CopyTo(wordReadOffset, res, 0, 2);
                wordReadOffset += 2;
            }
            else if (count >= 3)
            {
                data.CopyTo(Offset, res, 0, count);
                Offset += count;
                RealignReads();
            }

            return res;
        }

        public byte[] TakeBytesAligned(int count)
        {
            byte[] res = TakeBytes(count);
            RealignReads();
            return res;
        }

        public byte[] TakeBytesEndian(int count)
        {
            byte[] res = TakeBytes(count);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(res);
            return res;
        }

        public byte TakeU8()
        {
            return data[Offset++];
        }

        public sbyte TakeS8()
        {
            return unchecked((sbyte)data[Offset++]);
        }

        public ushort TakeU16()
        {
            byte[] bytes = TakeBytesEndian(2);
            return BitConverter.ToUInt16(bytes, 0);
        }

        public short TakeS16()
        {
            byte[] bytes = TakeBytesEndian(2);
            return BitConverter.ToInt16(bytes, 0);
        }

        public uint TakeU32()
        {
            byte[] bytes = TakeBytesEndian(4);
            return BitConverter.ToUInt32(bytes, 0);
        }

        public int TakeS32()
        {
            byte[] bytes = TakeBytesEndian(4);
            return BitConverter.ToInt32(bytes, 0);
        }

        public ulong TakeU64()
        {
            byte[] bytes = TakeBytesEndian(8);
            return BitConverter.ToUInt64(bytes, 0);
        }

        public long TakeS64()
        {
            byte[] bytes = TakeBytesEndian(8);
            return BitConverter.ToInt64(bytes, 0);
        }

        public float TakeFloat()
        {
            byte[] bytes = TakeBytesEndian(4);
            return BitConverter.ToSingle(bytes, 0);
        }

        public double TakeDouble()
        {
            byte[] bytes = TakeBytesEndian(8);
            return BitConverter.ToDouble(bytes, 0);
        }

        public string TakeString(Encoding encoding)
        {
            int length = TakeS32();
            byte[] bytes = TakeBytes(length);
            return encoding.GetString(bytes, 0, length - 1);
        }

        public void AddBytes(IEnumerable<byte> bytes)
        {
            data.AddRange(bytes);
        }

        public void AddBytesSubAligned(byte[] bytes)
        {
            if (bytes.Length == 1)
            {
                if (byteWriteOffset % align == 0)
                {
                    byteWriteOffset = data.Count;
                    data.AddRange(new byte[align]);
                }
                data[byteWriteOffset++] = bytes[0];
            }
            else if (bytes.Length == 2)
            {
                if (wordWriteOffset % align == 0)
                {
                    wordWriteOffset = data.Count;
                    data.AddRange(new byte[align]);
                }
                data[wordWriteOffset++] = bytes[0];
                data[wordWriteOffset++] = bytes[1];
            }
            else if(bytes.Length >= 3)
            {
                data.AddRange(bytes);
                RealignWrites();
            }
        }

        public void AddBytesAligned(byte[] bytes)
        {
            data.AddRange(bytes);
            RealignWrites();
        }

        public void AddBytesEndian(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            data.AddRange(bytes);
        }

        public void AddU8(byte input)
        {
            data.Add(input);
        }

        public void AddS8(sbyte input)
        {
            data.Add(unchecked((byte)input));
        }

        public void AddU16(ushort input)
        {
            AddBytesEndian(BitConverter.GetBytes(input));
        }

        public void AddS16(short input)
        {
            AddBytesEndian(BitConverter.GetBytes(input));
        }

        public void AddU32(uint input)
        {
            AddBytesEndian(BitConverter.GetBytes(input));
        }

        public void AddS32(int input)
        {
            AddBytesEndian(BitConverter.GetBytes(input));
        }

        public void AddU64(ulong input)
        {
            AddBytesEndian(BitConverter.GetBytes(input));
        }

        public void AddS64(long input)
        {
            AddBytesEndian(BitConverter.GetBytes(input));
        }

        public void AddFloat(float input)
        {
            AddBytesEndian(BitConverter.GetBytes(input));
        }

        public void AddDouble(double input)
        {
            AddBytesEndian(BitConverter.GetBytes(input));
        }

        public void AddString(string input, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(input);
            AddS32(bytes.Length + 1);
            AddBytes(bytes);
            AddU8(0);
        }
    }
}
