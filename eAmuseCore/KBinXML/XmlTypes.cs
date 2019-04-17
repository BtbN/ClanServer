﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Net;
using System.Text;
using System.Collections;

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
            throw new NotImplementedException();
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
    public class Time : KValue<DateTimeOffset>
    {
        public Time(DateTimeOffset value) => Value = value;

        public override string ToString() => Value.ToString();

        public override IEnumerable<byte> ToBytes()
        {
            IEnumerable<byte> res = BitConverter.GetBytes(Value.ToUnixTimeSeconds());
            if (BitConverter.IsLittleEndian)
                res = res.Reverse();
            return res.Skip(4);
        }

        static public Time FromString(string input)
        {
            return new Time(DateTimeOffset.Parse(input));
        }

        static public Time FromBytes(IEnumerable<byte> input)
        {
            uint seconds = input.FirstU32();
            return new Time(DateTimeOffset.FromUnixTimeSeconds(seconds));
        }
    }

    [KValue(27, "3u8", Count = 3, Size = 1)]
    public class K3U8 : KValueList<U8>
    {
        public K3U8(U8 v1, U8 v2, U8 v3)
        {
            Add(v1);
            Add(v2);
            Add(v3);
        }

        static public K3U8 FromString(string input)
        {
            string[] fields = input.Split(' ');
            if (fields.Length != 3)
                throw new ArgumentException("input string had invalid field count", "input");
            return new K3U8(
                U8.FromString(fields[0]),
                U8.FromString(fields[1]),
                U8.FromString(fields[2]));
        }

        static public K3U8 FromBytes(IEnumerable<byte> input)
        {
            return new K3U8(
                U8.FromBytes(input.Skip(0)),
                U8.FromBytes(input.Skip(1)),
                U8.FromBytes(input.Skip(2)));
        }
    }

    public static class XmlTypes
    {
        public const int NodeStartType = 1;
        public const int BinType = 10;
        public const int StrType = 11;
        public const int AttrType = 46;
        public const int NodeEndType = 190;
        public const int SectionEndType = 191;

        private static readonly Dictionary<byte, Type> lookupMap = new Dictionary<byte, Type>();

        public static void RegisterAll()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                KValueAttribute attr = type.GetCustomAttribute<KValueAttribute>(true);
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
                Type listType = typeof(KValueList<>).MakeGenericType(valType);
                IList list = (IList)Activator.CreateInstance(listType);
                int size = attrs.Size * attrs.Count;

                for (int i = 0; i < count; i++)
                {
                    list.Add(fromBytes.Invoke(null, new object[] { data }));
                    data = data.Skip(size);
                }

                return list;
            }
        }
    }
}
