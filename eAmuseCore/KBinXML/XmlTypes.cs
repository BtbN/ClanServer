using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Net;
using System.Text;
using System.Collections;
using System.Globalization;

namespace eAmuseCore.KBinXML.XmlTypes
{
#pragma warning disable IDE0060 // Remove unused parameter
    [KValue(1, "void", Count = 1, Size = 0)]
    public class Void : KValue<object>
    {
        public Void(object _) => Value = null;
        static public Void FromString(string input) => new Void(null);

        static public Void FromBytes(IEnumerable<byte> input) => new Void(null);
    }
#pragma warning restore IDE0060

    [KValue(2, "s8", Count = 1, Size = 1)]
    public class S8 : KValue<sbyte>
    {
        public S8(sbyte value) => Value = value;
        static public S8 FromString(string input) => new S8(Convert.ToSByte(input));
        static public S8 FromBytes(IEnumerable<byte> input) => new S8(input.FirstS8());
    }

    [KValue(3, "u8", Count = 1, Size = 1)]
    public class U8 : KValue<byte>
    {
        public U8(byte value) => Value = value;
        static public U8 FromString(string input) => new U8(Convert.ToByte(input));
        static public U8 FromBytes(IEnumerable<byte> input) => new U8(input.FirstU8());
    }

    [KValue(4, "s16", Count = 1, Size = 2)]
    public class S16 : KValue<short>
    {
        public S16(short value) => Value = value;
        static public S16 FromString(string input) => new S16(Convert.ToInt16(input));
        static public S16 FromBytes(IEnumerable<byte> input) => new S16(input.FirstS16());
    }

    [KValue(5, "u16", Count = 1, Size = 2)]
    public class U16 : KValue<ushort>
    {
        public U16(ushort value) => Value = value;
        static public U16 FromString(string input) => new U16(Convert.ToUInt16(input));
        static public U16 FromBytes(IEnumerable<byte> input) => new U16(input.FirstU16());
    }

    [KValue(6, "s32", Count = 1, Size = 4)]
    public class S32 : KValue<int>
    {
        public S32(int value) => Value = value;
        static public S32 FromString(string input) => new S32(Convert.ToInt32(input));
        static public S32 FromBytes(IEnumerable<byte> input) => new S32(input.FirstS32());
    }

    [KValue(7, "u32", Count = 1, Size = 4)]
    public class U32 : KValue<uint>
    {
        public U32(uint value) => Value = value;
        static public U32 FromString(string input) => new U32(Convert.ToUInt32(input));
        static public U32 FromBytes(IEnumerable<byte> input) => new U32(input.FirstU32());
    }

    [KValue(8, "s64", Count = 1, Size = 8)]
    public class S64 : KValue<long>
    {
        public S64(long value) => Value = value;
        static public S64 FromString(string input) => new S64(Convert.ToInt64(input));
        static public S64 FromBytes(IEnumerable<byte> input) => new S64(input.FirstS64());
    }

    [KValue(9, "u64", Count = 1, Size = 8)]
    public class U64 : KValue<ulong>
    {
        public U64(ulong value) => Value = value;
        static public U64 FromString(string input) => new U64(Convert.ToUInt64(input));
        static public U64 FromBytes(IEnumerable<byte> input) => new U64(input.FirstU64());
    }

    [KValue(10, "bin", "binary", Count = -1, Size = 1)]
    public class Bin : KValue<byte[]>
    {
        public Bin(byte[] value) => Value = value;

        public override string ToString()
        {
            return string.Join("", Value.Select(b => Convert.ToString(b, 16).PadLeft(2, '0')));
        }

        public override IEnumerable<byte> ToBytes() => Value;

        static public Bin FromString(string input)
        {
            if ((input.Length % 2) != 2)
                throw new ArgumentException("Hex string needs to consist of pairs of two chars.", "input");
            byte[] res = new byte[input.Length / 2];
            for (int i = 0, j = 0; i < input.Length; i += 2, ++j)
                res[j] = Convert.ToByte(input.Substring(i, 2), 16);
            return new Bin(res);
        }

        static public Bin FromBytes(IEnumerable<byte> input) => new Bin(input.ToArray());
    }

    [KValue(11, "str", "string", Count = -1, Size = 1)]
    public class Str : KValue<string>
    {
        public Str(string value) => Value = value;

        public override string ToString() => Value;

        public override IEnumerable<byte> ToBytes() => Encoding.UTF8.GetBytes(Value);

        static public Str FromString(string input) => new Str(input);

        static public Str FromBytes(IEnumerable<byte> input, Encoding encoding)
        {
            byte[] data = input.ToArray();
            return new Str(encoding.GetString(data, 0, data.Length - 1));
        }
    }

    [KValue(12, "ip4", Count = 1, Size = 4)]
    public class IP4 : KValue<IPAddress>
    {
        public IP4(IPAddress value) => Value = value;

        public override string ToString() => Value.ToString();

        public override IEnumerable<byte> ToBytes() => Value.GetAddressBytes();

        static public IP4 FromString(string input) => new IP4(IPAddress.Parse(input));

        static public IP4 FromBytes(IEnumerable<byte> input) => new IP4(new IPAddress(input.Take(4).ToArray()));
    }

    [KValue(13, "time", Count = 1, Size = 4)]
    public class Time : KValue<uint>
    {
        public Time(uint value) => Value = value;
        static public Time FromString(string input) => new Time(Convert.ToUInt32(input));
        static public Time FromBytes(IEnumerable<byte> input) => new Time(input.FirstU32());
    }

    [KValue(14, "float", "f", Count = 1, Size = 4)]
    public class KFloat : KValue<float>
    {
        public KFloat(float value) => Value = value;

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public override IEnumerable<byte> ToBytes()
        {
            IEnumerable<byte> res = BitConverter.GetBytes(Value);
            if (BitConverter.IsLittleEndian)
                res = res.Reverse();
            return res;
        }

        static public KFloat FromString(string input) => new KFloat(Convert.ToSingle(input, CultureInfo.InvariantCulture));

        static public KFloat FromBytes(IEnumerable<byte> input) => new KFloat(input.FirstF());
    }

    [KValue(15, "double", "d", Count = 1, Size = 8)]
    public class KDouble : KValue<double>
    {
        public KDouble(double value) => Value = value;

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public override IEnumerable<byte> ToBytes()
        {
            IEnumerable<byte> res = BitConverter.GetBytes(Value);
            if (BitConverter.IsLittleEndian)
                res = res.Reverse();
            return res;
        }

        static public KDouble FromString(string input) => new KDouble(Convert.ToDouble(input, CultureInfo.InvariantCulture));

        static public KDouble FromBytes(IEnumerable<byte> input) => new KDouble(input.FirstD());
    }

    [KValue(16, "2s8", Count = 2, Size = 1)]
    public class K2S8 : KValueArray<S8>
    {
        public K2S8(S8 v1, S8 v2) : base(v1, v2) { }
        static public K2S8 FromString(string input) => XmlTypes.ValueListTypeFromString<K2S8>(input);
        static public K2S8 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K2S8>(input);
    }

    [KValue(17, "2u8", Count = 2, Size = 1)]
    public class K2U8 : KValueArray<U8>
    {
        public K2U8(U8 v1, U8 v2) : base(v1, v2) { }
        static public K2U8 FromString(string input) => XmlTypes.ValueListTypeFromString<K2U8>(input);
        static public K2U8 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K2U8>(input);
    }

    [KValue(18, "2s16", Count = 2, Size = 2)]
    public class K2S16 : KValueArray<S16>
    {
        public K2S16(S16 v1, S16 v2) : base(v1, v2) { }
        static public K2S16 FromString(string input) => XmlTypes.ValueListTypeFromString<K2S16>(input);
        static public K2S16 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K2S16>(input);
    }

    [KValue(19, "2u16", Count = 2, Size = 2)]
    public class K2U16 : KValueArray<U16>
    {
        public K2U16(U16 v1, U16 v2) : base(v1, v2) { }
        static public K2U16 FromString(string input) => XmlTypes.ValueListTypeFromString<K2U16>(input);
        static public K2U16 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K2U16>(input);
    }

    [KValue(20, "2s32", Count = 2, Size = 4)]
    public class K2S32 : KValueArray<S32>
    {
        public K2S32(S32 v1, S32 v2) : base(v1, v2) { }
        static public K2S32 FromString(string input) => XmlTypes.ValueListTypeFromString<K2S32>(input);
        static public K2S32 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K2S32>(input);
    }

    [KValue(21, "2u32", Count = 2, Size = 4)]
    public class K2U32 : KValueArray<U32>
    {
        public K2U32(U32 v1, U32 v2) : base(v1, v2) { }
        static public K2U32 FromString(string input) => XmlTypes.ValueListTypeFromString<K2U32>(input);
        static public K2U32 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K2U32>(input);
    }

    [KValue(22, "2s64", "vs64", Count = 2, Size = 8)]
    public class K2S64 : KValueArray<S64>
    {
        public K2S64(S64 v1, S64 v2) : base(v1, v2) { }
        static public K2S64 FromString(string input) => XmlTypes.ValueListTypeFromString<K2S64>(input);
        static public K2S64 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K2S64>(input);
    }

    [KValue(23, "2u64", "vu64", Count = 2, Size = 8)]
    public class K2U64 : KValueArray<U64>
    {
        public K2U64(U64 v1, U64 v2) : base(v1, v2) { }
        static public K2U64 FromString(string input) => XmlTypes.ValueListTypeFromString<K2U64>(input);
        static public K2U64 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K2U64>(input);
    }

    [KValue(24, "2f", Count = 2, Size = 4)]
    public class K2F : KValueArray<KFloat>
    {
        public K2F(KFloat v1, KFloat v2) : base(v1, v2) { }
        static public K2F FromString(string input) => XmlTypes.ValueListTypeFromString<K2F>(input);
        static public K2F FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K2F>(input);
    }

    [KValue(25, "2d", "vd", Count = 2, Size = 8)]
    public class K2D : KValueArray<KDouble>
    {
        public K2D(KDouble v1, KDouble v2) : base(v1, v2) { }
        static public K2D FromString(string input) => XmlTypes.ValueListTypeFromString<K2D>(input);
        static public K2D FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K2D>(input);
    }

    [KValue(26, "3s8", Count = 3, Size = 1)]
    public class K3S8 : KValueArray<S8>
    {
        public K3S8(S8 v1, S8 v2, S8 v3) : base(v1, v2, v3) { }
        static public K3S8 FromString(string input) => XmlTypes.ValueListTypeFromString<K3S8>(input);
        static public K3S8 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K3S8>(input);
    }

    [KValue(27, "3u8", Count = 3, Size = 1)]
    public class K3U8 : KValueArray<U8>
    {
        public K3U8(U8 v1, U8 v2, U8 v3) : base(v1, v2, v3) { }
        static public K3U8 FromString(string input) => XmlTypes.ValueListTypeFromString<K3U8>(input);
        static public K3U8 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K3U8>(input);
    }

    [KValue(28, "3s16", Count = 3, Size = 2)]
    public class K3S16 : KValueArray<S16>
    {
        public K3S16(S16 v1, S16 v2, S16 v3) : base(v1, v2, v3) { }
        static public K3S16 FromString(string input) => XmlTypes.ValueListTypeFromString<K3S16>(input);
        static public K3S16 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K3S16>(input);
    }

    [KValue(29, "3u16", Count = 3, Size = 2)]
    public class K3U16 : KValueArray<U16>
    {
        public K3U16(U16 v1, U16 v2, U16 v3) : base(v1, v2, v3) { }
        static public K3U16 FromString(string input) => XmlTypes.ValueListTypeFromString<K3U16>(input);
        static public K3U16 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K3U16>(input);
    }

    [KValue(30, "3s32", Count = 3, Size = 4)]
    public class K3S32 : KValueArray<S32>
    {
        public K3S32(S32 v1, S32 v2, S32 v3) : base(v1, v2, v3) { }
        static public K3S32 FromString(string input) => XmlTypes.ValueListTypeFromString<K3S32>(input);
        static public K3S32 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K3S32>(input);
    }

    [KValue(31, "3u32", Count = 3, Size = 4)]
    public class K3U32 : KValueArray<U32>
    {
        public K3U32(U32 v1, U32 v2, U32 v3) : base(v1, v2, v3) { }
        static public K3U32 FromString(string input) => XmlTypes.ValueListTypeFromString<K3U32>(input);
        static public K3U32 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K3U32>(input);
    }

    [KValue(32, "3s64", Count = 3, Size = 8)]
    public class K3S64 : KValueArray<S64>
    {
        public K3S64(S64 v1, S64 v2, S64 v3) : base(v1, v2, v3) { }
        static public K3S64 FromString(string input) => XmlTypes.ValueListTypeFromString<K3S64>(input);
        static public K3S64 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K3S64>(input);
    }

    [KValue(33, "3u64", Count = 3, Size = 8)]
    public class K3U64 : KValueArray<U64>
    {
        public K3U64(U64 v1, U64 v2, U64 v3) : base(v1, v2, v3) { }
        static public K3U64 FromString(string input) => XmlTypes.ValueListTypeFromString<K3U64>(input);
        static public K3U64 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K3U64>(input);
    }

    [KValue(34, "3f", Count = 3, Size = 4)]
    public class K3F : KValueArray<KFloat>
    {
        public K3F(KFloat v1, KFloat v2, KFloat v3) : base(v1, v2, v3) { }
        static public K3F FromString(string input) => XmlTypes.ValueListTypeFromString<K3F>(input);
        static public K3F FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K3F>(input);
    }

    [KValue(35, "3d", Count = 3, Size = 8)]
    public class K3D : KValueArray<KDouble>
    {
        public K3D(KDouble v1, KDouble v2, KDouble v3) : base(v1, v2, v3) { }
        static public K3D FromString(string input) => XmlTypes.ValueListTypeFromString<K3D>(input);
        static public K3D FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K3D>(input);
    }

    [KValue(36, "4s8", Count = 4, Size = 1)]
    public class K4S8 : KValueArray<S8>
    {
        public K4S8(S8 v1, S8 v2, S8 v3, S8 v4) : base(v1, v2, v3, v4) { }
        static public K4S8 FromString(string input) => XmlTypes.ValueListTypeFromString<K4S8>(input);
        static public K4S8 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K4S8>(input);
    }

    [KValue(37, "4u8", Count = 4, Size = 1)]
    public class K4U8 : KValueArray<U8>
    {
        public K4U8(U8 v1, U8 v2, U8 v3, U8 v4) : base(v1, v2, v3, v4) { }
        static public K4U8 FromString(string input) => XmlTypes.ValueListTypeFromString<K4U8>(input);
        static public K4U8 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K4U8>(input);
    }

    [KValue(38, "4s16", Count = 4, Size = 2)]
    public class K4S16 : KValueArray<S16>
    {
        public K4S16(S16 v1, S16 v2, S16 v3, S16 v4) : base(v1, v2, v3, v4) { }
        static public K4S16 FromString(string input) => XmlTypes.ValueListTypeFromString<K4S16>(input);
        static public K4S16 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K4S16>(input);
    }

    [KValue(39, "4u16", Count = 4, Size = 2)]
    public class K4U16 : KValueArray<U16>
    {
        public K4U16(U16 v1, U16 v2, U16 v3, U16 v4) : base(v1, v2, v3, v4) { }
        static public K4U16 FromString(string input) => XmlTypes.ValueListTypeFromString<K4U16>(input);
        static public K4U16 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K4U16>(input);
    }

    [KValue(40, "4s32", "vs32", Count = 4, Size = 4)]
    public class K4S32 : KValueArray<S32>
    {
        public K4S32(S32 v1, S32 v2, S32 v3, S32 v4) : base(v1, v2, v3, v4) { }
        static public K4S32 FromString(string input) => XmlTypes.ValueListTypeFromString<K4S32>(input);
        static public K4S32 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K4S32>(input);
    }

    [KValue(41, "4u32", "vu32", Count = 4, Size = 4)]
    public class K4U32 : KValueArray<U32>
    {
        public K4U32(U32 v1, U32 v2, U32 v3, U32 v4) : base(v1, v2, v3, v4) { }
        static public K4U32 FromString(string input) => XmlTypes.ValueListTypeFromString<K4U32>(input);
        static public K4U32 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K4U32>(input);
    }

    [KValue(42, "4s64", Count = 4, Size = 8)]
    public class K4S64 : KValueArray<S64>
    {
        public K4S64(S64 v1, S64 v2, S64 v3, S64 v4) : base(v1, v2, v3, v4) { }
        static public K4S64 FromString(string input) => XmlTypes.ValueListTypeFromString<K4S64>(input);
        static public K4S64 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K4S64>(input);
    }

    [KValue(43, "4u64", Count = 4, Size = 8)]
    public class K4U64 : KValueArray<U64>
    {
        public K4U64(U64 v1, U64 v2, U64 v3, U64 v4) : base(v1, v2, v3, v4) { }
        static public K4U64 FromString(string input) => XmlTypes.ValueListTypeFromString<K4U64>(input);
        static public K4U64 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K4U64>(input);
    }

    [KValue(44, "4f", "vf", Count = 4, Size = 4)]
    public class K4F : KValueArray<KFloat>
    {
        public K4F(KFloat v1, KFloat v2, KFloat v3, KFloat v4) : base(v1, v2, v3, v4) { }
        static public K4F FromString(string input) => XmlTypes.ValueListTypeFromString<K4F>(input);
        static public K4F FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K4F>(input);
    }

    [KValue(45, "4d", Count = 4, Size = 8)]
    public class K4D : KValueArray<KDouble>
    {
        public K4D(KDouble v1, KDouble v2, KDouble v3, KDouble v4) : base(v1, v2, v3, v4) { }
        static public K4D FromString(string input) => XmlTypes.ValueListTypeFromString<K4D>(input);
        static public K4D FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K4D>(input);
    }

    [KValue(48, "vs8", Count = 16, Size = 1)]
    public class KVS8 : KValueArray<S8>
    {
        public KVS8(S8 v1, S8 v2, S8 v3, S8 v4, S8 v5, S8 v6, S8 v7, S8 v8, S8 v9, S8 v10, S8 v11, S8 v12, S8 v13, S8 v14, S8 v15, S8 v16)
            : base(v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13, v14, v15, v16) { }
        static public KVS8 FromString(string input) => XmlTypes.ValueListTypeFromString<KVS8>(input);
        static public KVS8 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<KVS8>(input);
    }

    [KValue(49, "vu8", Count = 16, Size = 1)]
    public class KVU8 : KValueArray<U8>
    {
        public KVU8(U8 v1, U8 v2, U8 v3, U8 v4, U8 v5, U8 v6, U8 v7, U8 v8, U8 v9, U8 v10, U8 v11, U8 v12, U8 v13, U8 v14, U8 v15, U8 v16)
            : base(v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13, v14, v15, v16) { }
        static public KVU8 FromString(string input) => XmlTypes.ValueListTypeFromString<KVU8>(input);
        static public KVU8 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<KVU8>(input);
    }

    [KValue(50, "vs16", Count = 8, Size = 2)]
    public class KVS16 : KValueArray<S16>
    {
        public KVS16(S16 v1, S16 v2, S16 v3, S16 v4, S16 v5, S16 v6, S16 v7, S16 v8)
            : base(v1, v2, v3, v4, v5, v6, v7, v8) { }
        static public KVS16 FromString(string input) => XmlTypes.ValueListTypeFromString<KVS16>(input);
        static public KVS16 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<KVS16>(input);
    }

    [KValue(51, "vu16", Count = 8, Size = 2)]
    public class KVU16 : KValueArray<U16>
    {
        public KVU16(U16 v1, U16 v2, U16 v3, U16 v4, U16 v5, U16 v6, U16 v7, U16 v8)
            : base(v1, v2, v3, v4, v5, v6, v7, v8) { }
        static public KVU16 FromString(string input) => XmlTypes.ValueListTypeFromString<KVU16>(input);
        static public KVU16 FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<KVU16>(input);
    }

    [KValue(52, "bool", "b", Count = 1, Size = 1)]
    public class KBool : KValue<bool>
    {
        public KBool(bool value) => Value = value;
        public override string ToString() => Value ? "1" : "0";
        public override IEnumerable<byte> ToBytes() => new byte[] { (byte)(Value ? 1 : 0) };

        static public KBool FromString(string input)
        {
            if (input == "0")
                return new KBool(false);
            else if (input == "1")
                return new KBool(true);
            else
                return new KBool(Convert.ToBoolean(input));
        }

        static public KBool FromBytes(IEnumerable<byte> input) => new KBool(input.FirstU8() != 0);
    }

    [KValue(53, "2b", Count = 2, Size = 1)]
    public class K2B : KValueArray<KBool>
    {
        public K2B(KBool v1, KBool v2) : base(v1, v2) { }
        static public K2B FromString(string input) => XmlTypes.ValueListTypeFromString<K2B>(input);
        static public K2B FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K2B>(input);
    }

    [KValue(54, "3b", Count = 3, Size = 1)]
    public class K3B : KValueArray<KBool>
    {
        public K3B(KBool v1, KBool v2, KBool v3) : base(v1, v2, v3) { }
        static public K3B FromString(string input) => XmlTypes.ValueListTypeFromString<K3B>(input);
        static public K3B FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K3B>(input);
    }

    [KValue(55, "4b", Count = 4, Size = 1)]
    public class K4B : KValueArray<KBool>
    {
        public K4B(KBool v1, KBool v2, KBool v3, KBool v4) : base(v1, v2, v3, v4) { }
        static public K4B FromString(string input) => XmlTypes.ValueListTypeFromString<K4B>(input);
        static public K4B FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<K4B>(input);
    }

    [KValue(56, "vb", Count = 16, Size = 1)]
    public class KVB : KValueArray<KBool>
    {
        public KVB(KBool v1, KBool v2, KBool v3, KBool v4, KBool v5, KBool v6, KBool v7, KBool v8, KBool v9, KBool v10, KBool v11, KBool v12, KBool v13, KBool v14, KBool v15, KBool v16)
            : base(v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13, v14, v15, v16) { }
        static public KVB FromString(string input) => XmlTypes.ValueListTypeFromString<KVB>(input);
        static public KVB FromBytes(IEnumerable<byte> input) => XmlTypes.ValueListTypeFromBytes<KVB>(input);
    }

    public static class XmlTypes
    {
        public const int NodeStartType = 1;
        public const int BinType = 10;
        public const int StrType = 11;
        public const int AttrType = 46;
        public const int ArrayType = 47;
        public const int NodeEndType = 190;
        public const int SectionEndType = 191;

        private static readonly Dictionary<byte, Type> lookupMap = new Dictionary<byte, Type>();

        public static void RegisterAll()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                KValueAttribute attr = type.GetCustomAttribute<KValueAttribute>(false);
                if (attr == null)
                    continue;

                lookupMap[attr.NodeType] = type;
                KValueAttribute.Register(attr);
            }
        }

        public static Type GetByType(byte type)
        {
            if (!lookupMap.ContainsKey(type))
                throw new NotImplementedException("KValue type not implemented: " + type);
            return lookupMap[type];
        }

        public static object MakeNodeFromBytes(KValueAttribute attrs, int count, IEnumerable<byte> data)
        {
            Type valType = GetByType(attrs.NodeType);
            MethodInfo fromBytes = valType.GetMethod("FromBytes", BindingFlags.Static | BindingFlags.Public);

            if (count <= 1)
            {
                return fromBytes.Invoke(null, new object[] { data });
            }
            else
            {
                Type listType = typeof(KValueArray<>).MakeGenericType(valType);
                int size = attrs.Size * attrs.Count;

                object[] p = new object[count];

                for (int i = 0; i < count; i++)
                {
                    p[i] = fromBytes.Invoke(null, new object[] { data });
                    data = data.Skip(size);
                }

                return Activator.CreateInstance(listType, p);
            }
        }

        public static IKValue ValueListFromString(Type type, string input, int count)
        {
            Type listType = typeof(KValueArray<>).MakeGenericType(type);
            KValueAttribute attr = type.GetCustomAttribute<KValueAttribute>(false);
            var fromString = type.GetMethod("FromString");

            string[] vals = input.Split(' ');
            IEnumerable<string> strings = vals.AsEnumerable();

            if (vals.Length != attr.Count * count)
                throw new ArgumentException("input string had invalid field count", "input");

            object[] mainParams = new object[count];

            for (int i = 0; i < count; ++i)
            {
                mainParams[i] = fromString.Invoke(null, new object[] { string.Join(" ", strings.Take(attr.Count)) });
                strings = strings.Skip(attr.Count);
            }

            return (IKValue)Activator.CreateInstance(listType, mainParams);
        }

        internal static T ValueListTypeFromString<T>(string input)
        {
            KValueAttribute attr = typeof(T).GetCustomAttribute<KValueAttribute>(false);
            Type valueType = typeof(T).BaseType.GetGenericArguments()[0];
            var fromString = valueType.GetMethod("FromString");

            string[] vals = input.Split(' ');

            if (vals.Length != attr.Count)
                throw new ArgumentException("input string had invalid field count", "input");

            object[] param = new object[attr.Count];
            for (int i = 0; i < attr.Count; ++i)
                param[i] = fromString.Invoke(null, new object[] { vals[i] });

            return (T)Activator.CreateInstance(typeof(T), param);
        }

        internal static T ValueListTypeFromBytes<T>(IEnumerable<byte> input)
        {
            KValueAttribute attr = typeof(T).GetCustomAttribute<KValueAttribute>(false);
            Type valueType = typeof(T).BaseType.GetGenericArguments()[0];
            var fromBytes = valueType.GetMethod("FromBytes");

            object[] values = new object[attr.Count];
            for (int i = 0; i < attr.Count; ++i)
            {
                values[i] = fromBytes.Invoke(null, new object[] { input });
                input = input.Skip(attr.Size);
            }

            return (T)Activator.CreateInstance(typeof(T), values);
        }
    }
}
