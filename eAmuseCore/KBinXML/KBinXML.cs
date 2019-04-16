using System;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;
using System.Collections.Generic;

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

        private XDocument doc;
        private Encoding encoding;

        public KBinXML(IEnumerable<byte> input)
        {
            
            Parse(input);
        }

        public KBinXML(XDocument doc)
        {
            this.doc = doc;
        }

        private void Parse(IEnumerable<byte> input)
        {
            doc = new XDocument();

            if (input.FirstU8() != SIGNATURE)
                throw new ArgumentException("Invalid signature", "input");
            input = input.Skip(1);

            bool compressed;
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

            encoding = GetEncoding(encodingSig);

            uint nodeEnd = input.FirstU32();
            input = input.Skip(4);

            IEnumerable<byte> nodeBuf = input.Take((int)nodeEnd);
            IEnumerable<byte> dataBuf = input.Skip((int)nodeEnd);

            ParseNodes(nodeBuf, dataBuf, compressed);
        }

        private void ParseNodes(IEnumerable<byte> nodeBuf, IEnumerable<byte> dataBuf, bool compressed)
        {
            IEnumerable<byte> dataByteBuf = Enumerable.Empty<byte>();
            IEnumerable<byte> dataWordBuf = Enumerable.Empty<byte>();

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

                string name = "";
                if (nodeType != XmlTypes.XmlTypes.NodeEndType && nodeType != XmlTypes.XmlTypes.SectionEndType)
                {
                    if (compressed)
                    {
                        name = SixBit.Unpack(ref nodeBuf);
                    }
                    else
                    {
                        int length = (nodeBuf.First() & ~64) + 1;
                        nodeBuf = nodeBuf.Skip(1);

                        byte[] nameBytes = nodeBuf.Take(length).ToArray();
                        nodeBuf = nodeBuf.Skip(length);

                        name = encoding.GetString(nameBytes);
                    }
                }

                KValueAttribute nodeAttrs = null;
                bool startNode = false;

                switch (nodeType)
                {
                    case XmlTypes.XmlTypes.AttrType:
                        string attrVal = EnumHelpers.TakeStringAligned(ref dataBuf, encoding);
                        node.SetAttributeValue(name, attrVal);
                        break;
                    case XmlTypes.XmlTypes.NodeEndType:
                        node = node.Parent;
                        break;
                    case XmlTypes.XmlTypes.SectionEndType:
                        nodesLeft = false;
                        break;
                    case XmlTypes.XmlTypes.NodeStartType:
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

                IEnumerable<byte> data = null;
                if (isArray || totSize > 2)
                {
                    data = EnumHelpers.TakeBytesAligned(ref dataBuf, totCount * nodeAttrs.Size);
                }
                else if (totSize == 1)
                {
                    if (!dataByteBuf.Any())
                    {
                        dataByteBuf = dataBuf;
                        dataBuf = dataBuf.Skip(4);
                    }
                    data = dataByteBuf.Take(1);
                    dataByteBuf = dataByteBuf.Skip(1);
                }
                else if (totSize == 2)
                {
                    if (!dataWordBuf.Any())
                    {
                        dataWordBuf = dataBuf;
                        dataBuf = dataBuf.Skip(4);
                    }
                    data = dataWordBuf.Take(2);
                    dataWordBuf = dataWordBuf.Skip(2);
                }
                else if (totSize == 0)
                {
                    continue;
                }

                if (nodeType == XmlTypes.XmlTypes.BinType)
                {
                    node.SetAttributeValue("__size", totCount);
                    node.SetValue(XmlTypes.Bin.FromBytes(data));
                }
                else if (nodeType == XmlTypes.XmlTypes.StrType)
                {
                    node.SetValue(XmlTypes.Str.FromBytes(data, encoding));
                }
                else
                {
                    node.SetValue(XmlTypes.XmlTypes.MakeNodeFromBytes(nodeType, arrCount, data));
                }
            }

            doc = new XDocument(fakeroot.FirstNode);
            Console.WriteLine(doc.ToString());
        }
    }
}
