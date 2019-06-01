using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace eAmuseCore.KBinXML
{
    public class KElement : XElement
    {
        public KElement(string name, string kType, params string[] vals)
            : base(name, new XAttribute("__type", kType))
        {
            if (vals.Length > 1)
                Add(new XAttribute("__count", vals.Length));

            Value = string.Join(" ", vals);
        }

        public KElement AddAttr(string name, object value)
        {
            Add(new XAttribute(name, value));
            return this;
        }
    }

    public class KStr : KElement
    {
        const string KType = "str";

        public KStr(string name, string value)
            : base(name, KType, value)
        {}
    }

    public class KBool : KElement
    {
        const string KType = "bool";

        public KBool(string name, bool value)
            : base(name, KType, value ? "1" : "0")
        {}
    }

    public class KIP4 : KElement
    {
        const string KType = "ip4";

        public KIP4(string name, System.Net.IPAddress address)
            : base(name, KType, address.MapToIPv4().ToString())
        {}
    }

    public class KInteger<T> : KElement
    {
        public KInteger(string name, string kType, T[] vals)
            : base(name, kType, Array.ConvertAll(vals, val => val.ToString()))
        {}

        public KInteger(string name, string kType, T val)
            : this(name, kType, new[] { val })
        {}

        public KInteger(string name, string kType, int cnt, T val)
            : this(name, kType, InitArray(cnt, val))
        {}

        private static T[] InitArray(int cnt, T val)
        {
            T[] res = new T[cnt];
            for (int i = 0; i < cnt; ++i)
                res[i] = val;
            return res;
        }
    }

    public class KS8 : KInteger<sbyte>
    {
        const string KType = "s8";

        public KS8(string name, sbyte[] vals)
            : base(name, KType, vals)
        {}

        public KS8(string name, sbyte val)
            : base(name, KType, val)
        {}

        public KS8(string name, int cnt, sbyte val)
            : base(name, KType, cnt, val)
        {}
    }

    public class KU8 : KInteger<byte>
    {
        const string KType = "u8";

        public KU8(string name, byte[] vals)
            : base(name, KType, vals)
        {}

        public KU8(string name, byte val)
            : base(name, KType, val)
        {}

        public KU8(string name, int cnt, byte val)
            : base(name, KType, cnt, val)
        {}
    }

    public class KS16 : KInteger<short>
    {
        const string KType = "s16";

        public KS16(string name, short[] vals)
            : base(name, KType, vals)
        { }

        public KS16(string name, short val)
            : base(name, KType, val)
        { }

        public KS16(string name, int cnt, short val)
            : base(name, KType, cnt, val)
        { }
    }

    public class KU16 : KInteger<ushort>
    {
        const string KType = "u16";

        public KU16(string name, ushort[] vals)
            : base(name, KType, vals)
        { }

        public KU16(string name, ushort val)
            : base(name, KType, val)
        { }

        public KU16(string name, int cnt, ushort val)
            : base(name, KType, cnt, val)
        { }
    }

    public class KS32 : KInteger<int>
    {
        const string KType = "s32";

        public KS32(string name, int[] vals)
            : base(name, KType, vals)
        { }

        public KS32(string name, int val)
            : base(name, KType, val)
        { }

        public KS32(string name, int cnt, int val)
            : base(name, KType, cnt, val)
        { }
    }

    public class KU32 : KInteger<uint>
    {
        const string KType = "u32";

        public KU32(string name, uint[] vals)
            : base(name, KType, vals)
        { }

        public KU32(string name, uint val)
            : base(name, KType, val)
        { }

        public KU32(string name, int cnt, uint val)
            : base(name, KType, cnt, val)
        { }
    }

    public class KS64 : KInteger<long>
    {
        const string KType = "s64";

        public KS64(string name, long[] vals)
            : base(name, KType, vals)
        { }

        public KS64(string name, long val)
            : base(name, KType, val)
        { }

        public KS64(string name, int cnt, long val)
            : base(name, KType, cnt, val)
        { }
    }

    public class KU64 : KInteger<ulong>
    {
        const string KType = "u64";

        public KU64(string name, ulong[] vals)
            : base(name, KType, vals)
        { }

        public KU64(string name, ulong val)
            : base(name, KType, val)
        { }

        public KU64(string name, int cnt, ulong val)
            : base(name, KType, cnt, val)
        { }
    }
}
