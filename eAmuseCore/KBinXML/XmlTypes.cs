using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Net;
using System.Text;
using System.Globalization;

using eAmuseCore.KBinXML.Helpers;

namespace eAmuseCore.KBinXML
{
    public delegate string KBinToString(IEnumerable<byte> input);
    public delegate IEnumerable<byte> KBinFromString(string input);

    public class KBinConverter
    {
        public KBinConverter(KBinFromString fromString, KBinToString toString)
        {
            KFromString = fromString;
            KToString = toString;
        }

        public KBinFromString KFromString { get; }
        public KBinToString KToString { get; }

        public static KBinConverter S8 = new KBinConverter(s => Convert.ToSByte(s).ToBytes(), b => b.FirstS8().ToString());
        public static KBinConverter U8 = new KBinConverter(s => Convert.ToByte(s).ToBytes(), b => b.FirstU8().ToString());
        public static KBinConverter S16 = new KBinConverter(s => Convert.ToInt16(s).ToBytes(), b => b.FirstS16().ToString());
        public static KBinConverter U16 = new KBinConverter(s => Convert.ToUInt16(s).ToBytes(), b => b.FirstU16().ToString());
        public static KBinConverter S32 = new KBinConverter(s => Convert.ToInt32(s).ToBytes(), b => b.FirstS32().ToString());
        public static KBinConverter U32 = new KBinConverter(s => Convert.ToUInt32(s).ToBytes(), b => b.FirstU32().ToString());
        public static KBinConverter S64 = new KBinConverter(s => Convert.ToInt64(s).ToBytes(), b => b.FirstS64().ToString());
        public static KBinConverter U64 = new KBinConverter(s => Convert.ToUInt64(s).ToBytes(), b => b.FirstU64().ToString());

        public static KBinConverter KFloat = new KBinConverter(s => Convert.ToSingle(s).ToBytes(), b => b.FirstF().ToString());
        public static KBinConverter KDouble = new KBinConverter(s => Convert.ToDouble(s).ToBytes(), b => b.FirstD().ToString());

        public static KBinConverter IP4 = new KBinConverter(s => IPAddress.Parse(s).GetAddressBytes(), b => new IPAddress(b.Take(4).ToArray()).ToString());
        public static KBinConverter Bool = new KBinConverter(s => new byte[] { s.ToBool() ? (byte)1 : (byte)0 }, b => (b.FirstU8() != 0) ? "1" : "0");

        public static KBinConverter Invalid = new KBinConverter(s => throw new InvalidOperationException(), b => throw new InvalidOperationException());
    }

    public class XmlType
    {
        public const int VoidType = 1;
        public const int BinType = 10;
        public const int StrType = 11;
        public const int AttrType = 46;
        public const int ArrayType = 47;
        public const int NodeEndType = 190;
        public const int SectionEndType = 191;

        public XmlType(int size, KBinConverter converter, params string[] names)
            : this(size, converter, 1, names)
        { }

        public XmlType(int size, KBinConverter converter, int count, params string[] names)
        {
            Size = size;
            Converter = converter;
            Count = count;
            Names = names;
        }

        public int Size { get; }
        private KBinConverter Converter { get; }
        public int Count { get; }
        public string[] Names { get; private set; }
        public string Name { get => Names[0]; }

        public XmlType Rename(params string[] names)
        {
            Names = names;
            return this;
        }

        public XmlType Alias(params string[] newNames)
        {
            Names = Names.Concat(newNames).ToArray();
            return this;
        }

        public IEnumerable<byte> KFromString(string input)
        {
            if (input.Length == 0)
                return new byte[0];
            return Converter.KFromString(input);
        }

        public string KToString(IEnumerable<byte> input)
        {
            input = input.Take(Size);
            ICollection<byte> col = input as ICollection<byte>;
            if (col != null && col.Count != Size)
                throw new ArgumentException("input does not provide enough data", "input");

            return Converter.KToString(input);
        }

        private XmlType Times(int count)
        {
            if (Count != 1)
                throw new InvalidOperationException("Multiplying already multiplied XmlType!");

            string[] names = Names.Select(name => count.ToString() + name).ToArray();
            int size = Size * count;
            KBinFromString fromString = s =>
            {
                string[] elems = s.Split(' ');
                if (elems.Length != count)
                    throw new ArgumentException("input does not split into correct element count", "s");

                IEnumerable<byte> res = Enumerable.Empty<byte>();
                foreach (string elem in elems)
                    res = res.Concat(Converter.KFromString(elem));
                return res;
            };
            KBinToString toString = b =>
            {
                IEnumerable<byte> data = b.Take(size);
                ICollection<byte> col = data as ICollection<byte>;
                if (col != null && col.Count != size)
                    throw new ArgumentException("input does not provide enough data for all elements", "b");

                string[] res = new string[count];
                for (int i = 0; i < count; ++i)
                {
                    res[i] = Converter.KToString(data.Take(Size));
                    data = data.Skip(Size);
                }
                return string.Join(" ", res);
            };

            return new XmlType(size, new KBinConverter(fromString, toString), count, names);
        }

        public static XmlType S8 = new XmlType(1, KBinConverter.S8, "s8");
        public static XmlType U8 = new XmlType(1, KBinConverter.U8, "u8");
        public static XmlType S16 = new XmlType(2, KBinConverter.S16, "s16");
        public static XmlType U16 = new XmlType(2, KBinConverter.U16, "u16");
        public static XmlType S32 = new XmlType(4, KBinConverter.S32, "s32");
        public static XmlType U32 = new XmlType(4, KBinConverter.U32, "u32");
        public static XmlType S64 = new XmlType(8, KBinConverter.S64, "s64");
        public static XmlType U64 = new XmlType(8, KBinConverter.U64, "u64");

        public static XmlType KFloat = new XmlType(4, KBinConverter.KFloat, "float", "f");
        public static XmlType KDouble = new XmlType(4, KBinConverter.KDouble, "double", "d");

        public static XmlType Bin = new XmlType(1, KBinConverter.Invalid, -1, "bin", "binary");
        public static XmlType Str = new XmlType(1, KBinConverter.Invalid, -1, "str", "string");

        public static XmlType IP4 = new XmlType(4, KBinConverter.IP4, "ip4");
        public static XmlType Time = new XmlType(4, KBinConverter.U32, "time");
        public static XmlType Bool = new XmlType(1, KBinConverter.Bool, "bool", "b");

        private static readonly Dictionary<byte, XmlType> lookupMap = new Dictionary<byte, XmlType>()
        {
            [2] = S8,
            [3] = U8,
            [4] = S16,
            [5] = U16,
            [6] = S32,
            [7] = U32,
            [8] = S64,
            [9] = U64,
            [10] = Bin,
            [11] = Str,
            [12] = IP4,
            [13] = Time,
            [14] = KFloat,
            [15] = KDouble,
            [16] = S8.Times(2),
            [17] = U8.Times(2),
            [18] = S16.Times(2),
            [19] = U16.Times(2),
            [20] = S32.Times(2),
            [21] = U32.Times(2),
            [22] = S64.Times(2).Alias("vs64"),
            [23] = U64.Times(2).Alias("vu64"),
            [24] = KFloat.Times(2).Rename("2f"),
            [25] = KDouble.Times(2).Rename("2d", "vd"),
            [26] = S8.Times(3),
            [27] = U8.Times(3),
            [28] = S16.Times(3),
            [29] = U16.Times(3),
            [30] = S32.Times(3),
            [31] = U32.Times(3),
            [32] = S64.Times(3),
            [33] = U64.Times(3),
            [34] = KFloat.Times(3).Rename("3f"),
            [35] = KDouble.Times(3).Rename("3d"),
            [36] = S8.Times(4),
            [37] = U8.Times(4),
            [38] = S16.Times(4),
            [39] = U16.Times(4),
            [40] = S32.Times(4).Alias("vs32"),
            [41] = U32.Times(4).Alias("vu32"),
            [42] = S64.Times(4),
            [43] = U64.Times(4),
            [44] = KFloat.Times(4).Rename("4f", "vf"),
            [45] = KDouble.Times(4).Rename("4d"),
            [48] = S8.Times(16).Rename("vs8"),
            [49] = U8.Times(16).Rename("vu8"),
            [50] = S16.Times(8).Rename("vs16"),
            [51] = U16.Times(8).Rename("vu16"),
            [52] = Bool,
            [53] = Bool.Times(2).Rename("2b"),
            [54] = Bool.Times(3).Rename("3b"),
            [55] = Bool.Times(4).Rename("4b"),
            [56] = Bool.Times(16).Rename("vb"),
        };

        private static readonly Dictionary<string, byte> reverseLookupMap = lookupMap
            .SelectMany(kvp => kvp.Value.Names.Select(name => new { k = name, v = kvp.Key }))
            .ToDictionary(kv => kv.k, kv => kv.v);

        public static XmlType GetByType(byte type)
        {
            if (!lookupMap.ContainsKey(type))
                throw new NotImplementedException("XmlType not implemented: " + type);
            return lookupMap[type];
        }

        public static byte GetIdByName(string name)
        {
            name = name.ToLower();
            if (!reverseLookupMap.ContainsKey(name))
                throw new NotImplementedException("XmlType not implemented: " + name);
            return reverseLookupMap[name];
        }
    }
}
