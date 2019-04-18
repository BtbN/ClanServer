using System;
using System.Collections.Generic;
using System.Linq;

namespace eAmuseCore.KBinXML.Helpers
{
    public interface IKValue
    {
        string ToString();
        IEnumerable<byte> ToBytes();
    }

    public class KValueArray<T> : IKValue where T : IKValue
    {
        private T[] values;

        public KValueArray(params T[] values)
        {
            this.values = values;
        }

        public T this[int idx]
        {
            get
            {
                return values[idx];
            }
            set
            {
                values[idx] = value;
            }
        }

        public IEnumerable<T> AsEnumerable()
        {
            return values.AsEnumerable();
        }

        public IEnumerable<byte> ToBytes()
        {
            IEnumerable<byte> res = Enumerable.Empty<byte>();
            foreach (IKValue val in values)
                res = res.Concat(val.ToBytes());
            return res;
        }

        public override string ToString()
        {
            return string.Join(" ", values.Select(v => v.ToString()));
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


        private static readonly Dictionary<string, KValueAttribute> nameLookupMap = new Dictionary<string, KValueAttribute>();
        private static readonly Dictionary<byte, KValueAttribute> typeLookupMap = new Dictionary<byte, KValueAttribute>();

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

    public class KValue<T> : IKValue
    {
        public T Value { get; set; }

        public KValue() { }
        public KValue(T value) => Value = value;

        public override string ToString()
        {
            return (Value != null) ? Value.ToString() : "";
        }

        public virtual IEnumerable<byte> ToBytes()
        {
            if (Value == null)
                return Enumerable.Empty<byte>();

            IEnumerable<byte> res = BitConverter.GetBytes(Convert.ToUInt64(Value));

            if (BitConverter.IsLittleEndian)
                res = res.Reverse();

            return res.Skip(8 - Size);
        }

        protected KValueAttribute KValAttr
        {
            get
            {
                var res = GetType().GetCustomAttributes(typeof(KValueAttribute), true).FirstOrDefault() as KValueAttribute;
                if (res == null)
                    throw new InvalidOperationException("KValue types need KValueAttributes!");
                return res;
            }
        }

        public int NodeType => KValAttr.NodeType;

        public string[] Names => KValAttr.Names;

        public string Name => KValAttr.Name;

        public int Count => KValAttr.Count;

        public int Size => KValAttr.Size;

        public Type ValueType => typeof(T);
    }
}
