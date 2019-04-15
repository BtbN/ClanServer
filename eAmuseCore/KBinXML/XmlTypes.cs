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
            public byte type;
            public int size;
            public int count;
            public string[] names;
        }

        public static readonly Entry NodeStart = new Entry
        {
            type = 1
        };

        public static readonly Entry Attr = new Entry
        {
            type = 46
        };

        public static readonly Entry NodeEnd = new Entry
        {
            type = 190
        };

        public static readonly Entry EndSection = new Entry
        {
            type = 191
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
                if (res == null || res.type != type)
                    continue;
                typeLookupMap[type] = res;
                return res;
            }

            return null;
        }
    }
}
