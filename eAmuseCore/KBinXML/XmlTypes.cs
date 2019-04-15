using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace eAmuseCore.KBinXML
{
    public static class XmlTypes
    {
        public const int NodeStartType = 1;
        public const int AttrType = 46;
        public const int NodeEndType = 190;
        public const int SectionEndType = 191;

        public class Entry
        {
            public string[] names = null;
            public byte nodeType = 0;
            public int size = 0;
            public int count = 0;
            public Type type = typeof(object);
            public Func<IEnumerable<byte>, string> toString;
        }

        public static readonly Entry Void = new Entry
        {
            names = new[] { "void" },
            nodeType = 1,
        };

        public static readonly Entry S8 = new Entry
        {
            names = new[] { "s8" },
            nodeType = 2,
            size = 1,
            count = 1,
            type = typeof(sbyte),
            toString = input => input.FirstS8().ToString(),
        };

        public static readonly Entry U8 = new Entry
        {
            names = new[] { "u8" },
            nodeType = 3,
            size = 1,
            count = 1,
            type = typeof(byte),
            toString = input => input.FirstU8().ToString(),
        };

        public static readonly Entry S16 = new Entry
        {
            names = new[] { "s16" },
            nodeType = 4,
            size = 2,
            count = 1,
            type = typeof(short),
            toString = input => input.FirstS16().ToString(),
        };

        public static readonly Entry U16 = new Entry
        {
            names = new[] { "u16" },
            nodeType = 5,
            size = 2,
            count = 1,
            type = typeof(ushort),
            toString = input => input.FirstU16().ToString(),
        };

        public static readonly Entry S32 = new Entry
        {
            names = new[] { "s32" },
            nodeType = 6,
            size = 4,
            count = 1,
            type = typeof(int),
            toString = input => input.FirstS32().ToString(),
        };

        public static readonly Entry U32 = new Entry
        {
            names = new[] { "u32" },
            nodeType = 7,
            size = 4,
            count = 1,
            type = typeof(int),
            toString = input => input.FirstU32().ToString(),
        };

        public static readonly Entry S64 = new Entry
        {
            names = new[] { "s64" },
            nodeType = 8,
            size = 8,
            count = 1,
            type = typeof(int),
            toString = input => input.FirstS64().ToString(),
        };

        public static readonly Entry U64 = new Entry
        {
            names = new[] { "u64" },
            nodeType = 9,
            size = 8,
            count = 1,
            type = typeof(int),
            toString = input => input.FirstU64().ToString(),
        };

        public static readonly Entry Bin = new Entry
        {
            names = new[] { "bin", "binary" },
            nodeType = 10,
            size = 1,
            count = -1,
            type = typeof(byte),
            toString = null, // special case, cannot be handled here
        };

        public static readonly Entry Str = new Entry
        {
            names = new[] { "str", "string" },
            nodeType = 11,
            size = 1,
            count = -1,
            type = typeof(byte),
            toString = null, // special case, cannot be handled here
        };

        private static Dictionary<string, Entry> nameLookupMap = new Dictionary<string, Entry>();
        private static Dictionary<byte, Entry> typeLookupMap = new Dictionary<byte, Entry>();

        public static Entry GetEntryByName(string name)
        {
            if (nameLookupMap.ContainsKey(name))
                return nameLookupMap[name];

            foreach (FieldInfo info in typeof(XmlTypes).GetFields())
            {
                if (!info.IsStatic)
                    continue;
                Entry res = info.GetValue(null) as Entry;
                if (res == null || !res.names.Contains(name))
                    continue;
                nameLookupMap[name] = res;
                return res;
            }

            return null;
        }

        public static Entry GetEntryByType(byte type)
        {
            if (typeLookupMap.ContainsKey(type))
                return typeLookupMap[type];

            foreach (FieldInfo info in typeof(XmlTypes).GetFields())
            {
                if (!info.IsStatic)
                    continue;
                Entry res = info.GetValue(null) as Entry;
                if (res == null || res.nodeType != type)
                    continue;
                typeLookupMap[type] = res;
                return res;
            }

            return null;
        }

        public static IEnumerable<string> DataToString(IEnumerable<byte> dataBuf, Entry entry)
        {
            while (dataBuf.Any())
            {
                yield return entry.toString(dataBuf);
                dataBuf = dataBuf.Skip(entry.size);
            }
        }
    }
}
