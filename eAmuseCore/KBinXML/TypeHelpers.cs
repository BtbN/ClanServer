using System;
using System.Collections.Generic;
using System.Linq;

namespace eAmuseCore.KBinXML
{
    public class KValueList<T> : List<T>
    {
        public override string ToString()
        {
            return string.Join(" ", this.Select(v => v.ToString()));
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public class KValueAttribute : Attribute
    {
        public KValueAttribute(byte nodeType, params string[] names)
        {
            NodeType = nodeType;
            Names = names;
        }

        public byte NodeType { get; }

        public string[] Names { get; }

        public string Name => Names[0];

        public int Count { get; set; }

        public int Size { get; set; }


        private static Dictionary<string, KValueAttribute> nameLookupMap = new Dictionary<string, KValueAttribute>();
        private static Dictionary<byte, KValueAttribute> typeLookupMap = new Dictionary<byte, KValueAttribute>();
        private static Dictionary<byte, Type> classLookupMap = new Dictionary<byte, Type>();

        public static void Register(KValueAttribute attr)
        {
            typeLookupMap[attr.NodeType] = attr;

            foreach (string name in attr.Names)
                nameLookupMap[name] = attr;
        }

        public static KValueAttribute GetAttrByName(string name)
        {
            if (!nameLookupMap.ContainsKey(name))
                throw new NotImplementedException("KValue name not implemented: " + name);
            return nameLookupMap[name];
        }

        public static KValueAttribute GetAttrByType(byte type)
        {
            if (!typeLookupMap.ContainsKey(type))
                throw new NotImplementedException("KValue type not implemented: " + type);
            return typeLookupMap[type];
        }
    }

    public class KValue<T>
    {
        public T Value { get; set; }

        public KValue() { }
        public KValue(T value) => Value = value;

        public override string ToString()
        {
            return (Value != null) ? Value.ToString() : "";
        }

        protected KValueAttribute KValAttr => GetType().GetCustomAttributes(typeof(KValueAttribute), true).First() as KValueAttribute;

        public int NodeType => KValAttr.NodeType;

        public string[] Names => KValAttr.Names;

        public string Name => KValAttr.Name;

        public int Count => KValAttr.Count;

        public int Size => KValAttr.Size;

        public Type ValueType => typeof(T);
    }
}
