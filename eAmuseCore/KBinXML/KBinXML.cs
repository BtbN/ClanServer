using System;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using eAmuseCore.KBinXML.Helpers;

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
            XmlTypes.XmlTypes.RegisterAll();
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
        public IEnumerable<byte> Bytes { get; private set; }
        public Encoding BinEncoding { get; private set; }

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

        public KBinXML(IEnumerable<byte> input)
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
            List<byte> header = new List<byte>(8);
            header.AddU8(SIGNATURE);

            if (compressed)
                header.AddU8(SIG_COMPRESSED);
            else
                header.AddU8(SIG_UNCOMPRESSED);

            byte encodingSig = GetEncodingSig(BinEncoding);
            header.AddU8(encodingSig);
            header.AddU8((byte)(0xFF ^ encodingSig));

            nodeList = new List<byte>();
            dataList = new List<byte>();
            dataByteOffset = dataWordOffset = 0;

            GenerateNode(Document.Root);

            nodeList.AddU8(XmlTypes.XmlTypes.SectionEndType | 64);
            nodeList.Realign();

            header.AddU32((uint)nodeList.Count);
            nodeList.AddU32((uint)dataList.Count);

            Bytes = header.Concat(nodeList).Concat(dataList).ToArray();

            nodeList = dataList = null;
        }

        private List<byte> nodeList = null, dataList = null;
        private int dataByteOffset = 0, dataWordOffset = 0;

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
                nodeList.AddRange(SixBit.Pack(name));
            }
            else
            {
                byte[] bytes = BinEncoding.GetBytes(name);
                nodeList.AddU8((byte)((bytes.Length - 1) | 64));
                nodeList.AddRange(bytes);
            }
        }

        private void AddDataAligned(byte[] data)
        {
            if (data.Length == 1)
            {
                if (dataByteOffset % 4 == 0)
                {
                    dataByteOffset = dataList.Count;
                    dataList.AddU32(0);
                }
                dataList[dataByteOffset++] = data[0];
            }
            else if (data.Length == 2)
            {
                if(dataWordOffset % 4 == 0)
                {
                    dataWordOffset = dataList.Count;
                    dataList.AddU32(0);
                }
                dataList[dataWordOffset++] = data[0];
                dataList[dataWordOffset++] = data[1];
            }
            else
            {
                dataList.AddRangeAligned(data);
            }
        }

        private void AddStringAligned(string str)
        {
            byte[] bytes = BinEncoding.GetBytes(str);
            dataList.AddS32(bytes.Length + 1);
            dataList.AddRange(bytes);
            dataList.AddU8(0);
            dataList.Realign();
        }

        private void GenerateNode(XElement node)
        {
            if (NodeIsMixed(node))
                throw new ArgumentException("Nodes with mixed elements/text are not supported.", "node");

            XAttribute nodeTypeXAttr = node.Attribute("__type");
            KValueAttribute nodeTypeAttrs;
            if (nodeTypeXAttr != null)
            {
                nodeTypeAttrs = KValueAttribute.GetAttrByName(nodeTypeXAttr.Value.ToLower());
            }
            else
            {
                if (node.IsEmpty || node.HasElements)
                    nodeTypeAttrs = KValueAttribute.GetAttrByType(XmlTypes.XmlTypes.VoidType);
                else
                    nodeTypeAttrs = KValueAttribute.GetAttrByType(XmlTypes.XmlTypes.StrType);
            }

            bool isArray = false;
            int count = 1;
            XAttribute countXAttr = node.Attribute("__count");
            if (countXAttr != null)
            {
                count = Convert.ToInt32(countXAttr.Value);
                isArray = true;
            }

            nodeList.AddU8((byte)(nodeTypeAttrs.NodeType | (isArray ? 64 : 0)));
            AddNodeName(node.Name.LocalName);

            if (nodeTypeAttrs.NodeType != XmlTypes.XmlTypes.VoidType)
            {
                Type valueType = XmlTypes.XmlTypes.GetByType(nodeTypeAttrs.NodeType);

                IEnumerable<byte> data;
                if (nodeTypeAttrs.NodeType == XmlTypes.XmlTypes.StrType)
                {
                    if (count != 1)
                        throw new FormatException("String value cannot have a count != 1.");

                    data = BinEncoding.GetBytes(node.Value).Concat(new byte[] { 0 });
                }
                else if (nodeTypeAttrs.NodeType == XmlTypes.XmlTypes.BinType)
                {
                    data = XmlTypes.Bin.FromString(node.Value).ToBytes();
                }
                else
                {
                    IKValue kValue = XmlTypes.XmlTypes.KValueFromString(valueType, node.Value, count);
                    data = kValue.ToBytes();
                }

                if (isArray || nodeTypeAttrs.Count < 0)
                {
                    dataList.AddU32((uint)data.Count());
                    dataList.AddRangeAligned(data);
                }
                else
                {
                    AddDataAligned(data.ToArray());
                }
            }

            foreach (XAttribute attr in node.Attributes())
            {
                if (new[] { "__type", "__size", "__count" }.Contains(attr.Name.LocalName))
                    continue;

                nodeList.AddU8(XmlTypes.XmlTypes.AttrType);
                AddNodeName(attr.Name.LocalName);

                AddStringAligned(attr.Value);
            }

            foreach (XElement child in node.Elements())
                GenerateNode(child);

            nodeList.AddU8(XmlTypes.XmlTypes.NodeEndType | 64);
        }

        private void Parse()
        {
            IEnumerable<byte> input = Bytes;
            Document = new XDocument();

            if (input.FirstU8() != SIGNATURE)
                throw new ArgumentException("Invalid signature", "input");
            input = input.Skip(1);

            switch (input.FirstU8())
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
            input = input.Skip(1);

            byte encodingSig = input.FirstU8();
            input = input.Skip(1);

            if (input.FirstU8() != (0xFF ^ encodingSig))
                throw new ArgumentException("Encoding signature failed to verify", "input");
            input = input.Skip(1);

            BinEncoding = GetEncoding(encodingSig);

            uint nodesSize = input.FirstU32();
            input = input.Skip(4);

            nodeBuf = input.Take((int)nodesSize);
            dataBuf = input.Skip((int)nodesSize);
            dataByteBuf = dataWordBuf = Enumerable.Empty<byte>();

            ParseNodes();

            nodeBuf = dataBuf = null;
            dataByteBuf = dataWordBuf = null;
            Bytes = Bytes.ToArray();
        }

        private IEnumerable<byte> nodeBuf = Enumerable.Empty<byte>(), dataBuf = Enumerable.Empty<byte>();
        private IEnumerable<byte> dataByteBuf = Enumerable.Empty<byte>(), dataWordBuf = Enumerable.Empty<byte>();
        bool compressed = false;

        IEnumerable<byte> TakeDataAligned(int size, bool isArray)
        {
            if (size <= 0)
            {
                return Enumerable.Empty<byte>();
            }
            else if (size == 1 && !isArray)
            {
                if (!dataByteBuf.Any())
                {
                    dataByteBuf = dataBuf.Take(4);
                    dataBuf = dataBuf.Skip(4);
                }
                var res = dataByteBuf.Take(1);
                dataByteBuf = dataByteBuf.Skip(1);
                return res;
            }
            else if (size == 2 && !isArray)
            {
                if (!dataWordBuf.Any())
                {
                    dataWordBuf = dataBuf.Take(4);
                    dataBuf = dataBuf.Skip(4);
                }
                var res = dataWordBuf.Take(2);
                dataWordBuf = dataWordBuf.Skip(2);
                return res;
            }
            else
            {
                return EnumHelpers.TakeBytesAligned(ref dataBuf, size);
            }
        }

        void SetNodeValue(XElement node, IEnumerable<byte> data, KValueAttribute nodeAttrs, int varCount, int arrCount)
        {
            if (nodeAttrs.NodeType == XmlTypes.XmlTypes.BinType)
            {
                node.SetAttributeValue("__size", varCount);
                node.SetValue(XmlTypes.Bin.FromBytes(data));
            }
            else if (nodeAttrs.NodeType == XmlTypes.XmlTypes.StrType)
            {
                node.SetValue(XmlTypes.Str.FromBytes(data, BinEncoding));
            }
            else
            {
                node.SetValue(XmlTypes.XmlTypes.MakeNodeFromBytes(nodeAttrs, arrCount, data));
            }
        }

        private string GetNodeName(byte nodeType)
        {
            if (nodeType != XmlTypes.XmlTypes.NodeEndType && nodeType != XmlTypes.XmlTypes.SectionEndType)
            {
                if (compressed)
                {
                    return SixBit.Unpack(ref nodeBuf);
                }
                else
                {
                    int length = (nodeBuf.First() & ~64) + 1;
                    nodeBuf = nodeBuf.Skip(1);

                    byte[] nameBytes = nodeBuf.Take(length).ToArray();
                    nodeBuf = nodeBuf.Skip(length);

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
            uint dataSize = dataBuf.FirstU32();
            dataBuf = dataBuf.Skip(4);

            XElement fakeroot = new XElement("fakeroot");
            XElement node = fakeroot;

            bool nodesLeft = true;
            while (nodesLeft && nodeBuf.Any())
            {
                nodeBuf = nodeBuf.SkipWhile(b => b == 0);

                byte nodeType = nodeBuf.FirstU8();
                nodeBuf = nodeBuf.Skip(1);

                bool isArray = (nodeType & 64) != 0;
                nodeType = (byte)(nodeType & ~64);

                string name = GetNodeName(nodeType);

                KValueAttribute nodeAttrs = null;
                bool startNode = false;

                switch (nodeType)
                {
                    case XmlTypes.XmlTypes.AttrType:
                        string attrVal = EnumHelpers.TakeStringAligned(ref dataBuf, BinEncoding);
                        node.SetAttributeValue(name, attrVal);
                        break;
                    case XmlTypes.XmlTypes.NodeEndType:
                        node = node.Parent;
                        break;
                    case XmlTypes.XmlTypes.SectionEndType:
                        nodesLeft = false;
                        break;
                    case XmlTypes.XmlTypes.VoidType:
                        startNode = true;
                        break;
                    default:
                        nodeAttrs = KValueAttribute.GetAttrByType(nodeType);
                        break;
                }

                if (nodeAttrs == null && !startNode)
                    continue;

                XElement child = new XElement(name);
                node.Add(child);
                node = child;

                if (startNode)
                    continue;

                node.SetAttributeValue("__type", nodeAttrs.Name);

                int varCount = nodeAttrs.Count;
                int arrCount = 1;
                if (varCount < 0)
                {
                    varCount = dataBuf.FirstS32();
                    dataBuf = dataBuf.Skip(4);
                    isArray = true;
                }
                else if (isArray)
                {
                    arrCount = dataBuf.FirstS32() / (nodeAttrs.Size * nodeAttrs.Count);
                    dataBuf = dataBuf.Skip(4);
                    node.SetAttributeValue("__count", arrCount);
                }
                int totCount = arrCount * varCount;
                int totSize = totCount * nodeAttrs.Size;

                IEnumerable<byte> data = TakeDataAligned(totSize, isArray);

                SetNodeValue(node, data, nodeAttrs, varCount, arrCount);
            }

            Document = new XDocument(fakeroot.FirstNode);
        }
    }
}
