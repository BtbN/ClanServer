using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace eAmuseCore.KBinXML
{
    public static class XmlTypes
    {
        public class Entry
        {
            public string[] names = null;
            public byte nodeType = 0;
            public int size = 0;
            public int count = 0;
            public Type type = typeof(object);
            public Func<IEnumerable<byte>, IEnumerable<object>> read = _ => Enumerable.Empty<object>();
            public bool isVirtual = false;
        }

        public static readonly Entry NodeStart = new Entry
        {
            names = new[] { "nodeStart" },
            nodeType = 1,
            isVirtual = true,
        };

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
            read = input => input.TakeS8(1).Box(),
        };

        public static readonly Entry U8 = new Entry
        {
            names = new[] { "u8" },
            nodeType = 3,
            size = 1,
            count = 1,
            type = typeof(byte),
            read = input => input.TakeU8(1).Box(),
        };

        public static readonly Entry S16 = new Entry
        {
            names = new[] { "s16" },
            nodeType = 4,
            size = 2,
            count = 1,
            type = typeof(short),
            read = input => input.TakeS16(1).Box(),
        };

        public static readonly Entry U16 = new Entry
        {
            names = new[] { "u16" },
            nodeType = 5,
            size = 2,
            count = 1,
            type = typeof(ushort),
            read = input => input.TakeU16(1).Box(),
        };

        public static readonly Entry S32 = new Entry
        {
            names = new[] { "s32" },
            nodeType = 6,
            size = 4,
            count = 1,
            type = typeof(int),
            read = input => input.TakeS32(1).Box(),
        };

        public static readonly Entry U32 = new Entry
        {
            names = new[] { "u32" },
            nodeType = 7,
            size = 4,
            count = 1,
            type = typeof(int),
            read = input => input.TakeU32(1).Box(),
        };

        public static readonly Entry S64 = new Entry
        {
            names = new[] { "s64" },
            nodeType = 8,
            size = 8,
            count = 1,
            type = typeof(int),
            read = input => input.TakeS64(1).Box(),
        };

        public static readonly Entry U64 = new Entry
        {
            names = new[] { "u64" },
            nodeType = 9,
            size = 8,
            count = 1,
            type = typeof(int),
            read = input => input.TakeU64(1).Box(),
        };

        public static readonly Entry Bin = new Entry
        {
            names = new[] { "bin", "binary" },
            nodeType = 10,
            size = 1,
            count = -1,
            type = typeof(byte[]),
            read = input => input.TakeU64(1).Box(),
        };

        public static readonly Entry Attr = new Entry
        {
            names = new[] { "attr" },
            nodeType = 46,
        };

        public static readonly Entry NodeEnd = new Entry
        {
            names = new[] { "nodeEnd" },
            nodeType = 190,
            isVirtual = true,
        };

        public static readonly Entry EndSection = new Entry
        {
            names = new[] { "endSection" },
            nodeType = 191,
            isVirtual = true,
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
                if (res == null || res.nodeType != type || res.isVirtual)
                    continue;
                typeLookupMap[type] = res;
                return res;
            }

            return null;
        }
    }
}
