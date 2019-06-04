using System;
using System.Text;
using System.Xml.Linq;

namespace eAmuseCore.KBinXML
{
    public class KBinXML
    {
        const byte SIGNATURE = 0xA0;
        const byte SIG_COMPRESSED = 0x42;
        const byte SIG_UNCOMPRESSED = 0x45;

        static KBinXML()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private static Encoding GetEncoding(byte sig)
        {
            switch (sig)
            {
                case 0x00:
                case 0x80:
                    return Encoding.GetEncoding(932);
                case 0x20:
                    return Encoding.ASCII;
                case 0x40:
                    return Encoding.GetEncoding("iso-8859-1");
                case 0x60:
                    return Encoding.GetEncoding("EUC-JP");
                case 0xA0:
                    return Encoding.UTF8;
            }

            throw new ArgumentException("Unsupported encoding", "sig");            
        }

        private static byte GetEncodingSig(Encoding enc)
        {
            if (Encoding.UTF8.Equals(enc))
                return 0xA0;
            if (Encoding.GetEncoding(932).Equals(enc))
                return 0x80;
            if (Encoding.ASCII.Equals(enc))
                return 0x20;
            if (Encoding.GetEncoding("iso-8859-1").Equals(enc))
                return 0x40;
            if (Encoding.GetEncoding("EUC-JP").Equals(enc))
                return 0x60;

            throw new ArgumentException("Unsupported encoding", "enc");
        }

        public XDocument Document { get; private set; }
        public byte[] Bytes { get; private set; }
        public Encoding BinEncoding { get; private set; }

        private ByteBuffer nodeBuf = null, dataBuf = null;
        bool compressed = false;

        public KBinXML(XDocument doc, Encoding encoding, bool compress = true)
        {
            Document = doc;
            compressed = compress;
            BinEncoding = encoding;

            Generate();
        }

        public KBinXML(XDocument doc, bool compress = true)
            :this(doc, Encoding.GetEncoding(932), compress)
        { }

        public KBinXML(byte[] input)
        {
            Bytes = input;

            Parse();
        }

        public override string ToString()
        {
            return Document.ToString();
        }

        private void Generate()
        {
            ByteBuffer header = new ByteBuffer();
            header.AddU8(SIGNATURE);

            if (compressed)
                header.AddU8(SIG_COMPRESSED);
            else
                header.AddU8(SIG_UNCOMPRESSED);

            byte encodingSig = GetEncodingSig(BinEncoding);
            header.AddU8(encodingSig);
            header.AddU8((byte)(0xFF ^ encodingSig));

            nodeBuf = new ByteBuffer();
            dataBuf = new ByteBuffer();

            GenerateNode(Document.Root);

            nodeBuf.AddU8(XmlType.SectionEndType | 64);
            nodeBuf.RealignWrites();

            header.AddU32((uint)nodeBuf.Length);
            nodeBuf.AddU32((uint)dataBuf.Length);

            byte[] bytes = new byte[header.Length + nodeBuf.Length + dataBuf.Length];

            header.CopyTo(0, bytes, 0, header.Length);
            nodeBuf.CopyTo(0, bytes, header.Length, nodeBuf.Length);
            dataBuf.CopyTo(0, bytes, header.Length + nodeBuf.Length, dataBuf.Length);

            Bytes = bytes;

            nodeBuf = dataBuf = null;
        }

        private bool NodeIsMixed(XElement element)
        {
            bool text = false;
            bool nontext = false;

            foreach (XNode node in element.Nodes())
            {
                if (node.NodeType == System.Xml.XmlNodeType.Text)
                    text = true;
                else
                    nontext = true;

                if (text && nontext)
                    return true;
            }

            return false;
        }

        private void AddNodeName(string name)
        {
            if (compressed)
            {
                nodeBuf.AddBytes(SixBit.Pack(name));
            }
            else
            {
                byte[] bytes = BinEncoding.GetBytes(name);
                nodeBuf.AddU8((byte)((bytes.Length - 1) | 64));
                nodeBuf.AddBytes(bytes);
            }
        }

        private byte[] GetNodeData(XElement node, byte nodeType, XmlType xmlType, int count)
        {
            if (nodeType == XmlType.StrType)
            {
                if (count != 1)
                    throw new FormatException("String value cannot have a count != 1.");

                byte[] res = BinEncoding.GetBytes(node.Value.Trim());
                Array.Resize(ref res, res.Length + 1);
                res[res.Length - 1] = 0;
                return res;
            }
            else if (nodeType == XmlType.BinType)
            {
                string val = node.Value;
                if ((val.Length % 2) != 0)
                    throw new ArgumentException("Hex string needs to consist of pairs of two chars.");

                byte[] res = new byte[val.Length / 2];
                for (int i = 0, j = 0; i < val.Length; i += 2, ++j)
                    res[j] = Convert.ToByte(val.Substring(i, 2), 16);

                return res;
            }
            else
            {
                string[] parts = node.Value.Split(' ');
                if (parts.Length != count * xmlType.Count)
                    throw new ArgumentException("Node value does not have required amount of fields.", "node");

                byte[] res = new byte[xmlType.Size * count];
                for (int i = 0; i < count; ++i)
                {
                    Buffer.BlockCopy(xmlType.KFromString(parts, i * xmlType.Count), 0, res, xmlType.Size * i, xmlType.Size);
                }

                return res;
            }
        }

        private void GenerateNode(XElement node)
        {
            if (NodeIsMixed(node))
                throw new ArgumentException("Nodes with mixed elements/text are not supported.", "node");

            XAttribute nodeTypeXAttr = node.Attribute("__type");
            byte nodeType;
            XmlType xmlType = null;
            if (nodeTypeXAttr != null)
            {
                nodeType = XmlType.GetIdByName(nodeTypeXAttr.Value.ToLower());
                xmlType = XmlType.GetByType(nodeType);
            }
            else
            {
                if (node.IsEmpty || node.HasElements)
                    nodeType = XmlType.VoidType;
                else
                    nodeType = XmlType.StrType;
            }

            bool isArray = false;
            int count = 1;
            XAttribute countXAttr = node.Attribute("__count");
            if (countXAttr != null)
            {
                count = Convert.ToInt32(countXAttr.Value);
                isArray = true;
            }

            nodeBuf.AddU8((byte)(nodeType | (isArray ? 64 : 0)));
            AddNodeName(node.Name.LocalName);

            if (nodeType != XmlType.VoidType)
            {
                byte[] data = GetNodeData(node, nodeType, xmlType, count);

                if (isArray || xmlType.Count < 0)
                {
                    dataBuf.AddS32(data.Length);
                    dataBuf.AddBytesAligned(data);
                }
                else
                {
                    dataBuf.AddBytesSubAligned(data);
                }
            }

            foreach (XAttribute attr in node.Attributes())
            {
                if (attr.Name.LocalName == "__type" || attr.Name.LocalName == "__size" || attr.Name.LocalName == "__count")
                    continue;

                nodeBuf.AddU8(XmlType.AttrType);
                AddNodeName(attr.Name.LocalName);

                dataBuf.AddString(attr.Value, BinEncoding);
                dataBuf.RealignWrites();
            }

            foreach (XElement child in node.Elements())
                GenerateNode(child);

            nodeBuf.AddU8(XmlType.NodeEndType | 64);
        }

        private void Parse()
        {
            ByteBuffer input = new ByteBuffer(Bytes);
            Document = new XDocument();

            if (input.TakeU8() != SIGNATURE)
                throw new ArgumentException("Invalid signature", "input");

            switch (input.TakeU8())
            {
                case SIG_COMPRESSED:
                    compressed = true;
                    break;
                case SIG_UNCOMPRESSED:
                    compressed = false;
                    break;
                default:
                    throw new ArgumentException("Invalud compression info", "input");
            }

            byte encodingSig = input.TakeU8();

            if (input.TakeU8() != (0xFF ^ encodingSig))
                throw new ArgumentException("Encoding signature failed to verify", "input");


            BinEncoding = GetEncoding(encodingSig);

            int nodesSize = input.TakeS32();

            nodeBuf = input.MakeSub(0, nodesSize);
            dataBuf = input.MakeSub(nodesSize);

            ParseNodes();

            nodeBuf = dataBuf = null;
        }

        void SetNodeValue(XElement node, byte nodeType, XmlType xmlType, bool isArray)
        {
            int varCount = xmlType.Count;
            int arrCount = 1;
            if (varCount < 0)
            {
                varCount = dataBuf.TakeS32();
                isArray = true;
            }
            else if (isArray)
            {
                arrCount = dataBuf.TakeS32() / (xmlType.Size * xmlType.Count);
                node.SetAttributeValue("__count", arrCount);
            }
            int totCount = arrCount * varCount;
            int totSize = totCount * xmlType.Size;

            byte[] data;
            if (isArray)
                data = dataBuf.TakeBytesAligned(totSize);
            else
                data = dataBuf.TakeBytesSubAligned(totSize);

            if (nodeType == XmlType.BinType)
            {
                node.SetAttributeValue("__size", data.Length);

                StringBuilder sb = new StringBuilder(data.Length * 2);
                for (int i = 0; i < data.Length; ++i)
                    sb.Append(Convert.ToString(data[i], 16).PadLeft(2, '0'));

                node.SetValue(sb.ToString());
            }
            else if (nodeType == XmlType.StrType)
            {
                node.SetValue(BinEncoding.GetString(data, 0, data.Length - 1));
            }
            else
            {
                string[] parts = new string[arrCount];
                for (int i = 0; i < arrCount; ++i)
                {
                    parts[i] = xmlType.KToString(data, i * xmlType.Size);
                }
                node.SetValue(string.Join(" ", parts));
            }
        }

        private string GetNodeName(byte nodeType)
        {
            if (nodeType != XmlType.NodeEndType && nodeType != XmlType.SectionEndType)
            {
                if (compressed)
                {
                    return SixBit.Unpack(nodeBuf);
                }
                else
                {
                    int length = (nodeBuf.TakeU8() & ~64) + 1;
                    byte[] nameBytes = nodeBuf.TakeBytes(length);

                    return BinEncoding.GetString(nameBytes);
                }
            }
            else
            {
                return "";
            }
        }

        private void ParseNodes()
        {
            uint dataSize = dataBuf.TakeU32();

            XElement fakeroot = new XElement("fakeroot");
            XElement node = fakeroot;

            bool nodesLeft = true;
            while (nodesLeft && !nodeBuf.AtEnd)
            {
                while (nodeBuf[nodeBuf.Offset] == 0)
                    nodeBuf.Offset += 1;

                byte nodeType = nodeBuf.TakeU8();

                bool isArray = (nodeType & 64) != 0;
                nodeType = (byte)(nodeType & ~64);

                string name = GetNodeName(nodeType);

                XmlType xmlType = null;
                bool startNode = false;

                switch (nodeType)
                {
                    case XmlType.AttrType:
                        string attrVal = dataBuf.TakeString(BinEncoding);
                        dataBuf.RealignReads();
                        node.SetAttributeValue(name, attrVal);
                        break;
                    case XmlType.NodeEndType:
                        node = node.Parent;
                        break;
                    case XmlType.SectionEndType:
                        nodesLeft = false;
                        break;
                    case XmlType.VoidType:
                        startNode = true;
                        break;
                    default:
                        xmlType = XmlType.GetByType(nodeType);
                        break;
                }

                if (xmlType == null && !startNode)
                    continue;

                XElement child = new XElement(name);
                node.Add(child);
                node = child;

                if (startNode)
                    continue;

                node.SetAttributeValue("__type", xmlType.Name);
                SetNodeValue(node, nodeType, xmlType, isArray);
            }

            Document = new XDocument(fakeroot.FirstNode);
        }
    }
}
