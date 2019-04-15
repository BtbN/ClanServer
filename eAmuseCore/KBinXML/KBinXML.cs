using System;
using System.Text;
using System.Linq;
using System.Xml.Linq;
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

            Console.WriteLine("Compressed: " + compressed);

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
                if (nodeType != XmlTypes.NodeEndType && nodeType != XmlTypes.SectionEndType)
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

                Console.WriteLine("Name: " + name);
                Console.WriteLine("Type: " + nodeType);

                XmlTypes.Entry nodeTypeEntry = null;
                bool startNode = false;

                switch (nodeType)
                {
                    case XmlTypes.AttrType:
                        string attrVal = EnumHelpers.TakeStringAligned(ref dataBuf, encoding);
                        node.SetAttributeValue(name, attrVal);
                        Console.WriteLine("Attr val: " + attrVal);
                        break;
                    case XmlTypes.NodeEndType:
                        node = node.Parent;
                        break;
                    case XmlTypes.SectionEndType:
                        nodesLeft = false;
                        break;
                    case XmlTypes.NodeStartType:
                        startNode = true;
                        break;
                    default:
                        nodeTypeEntry = XmlTypes.GetEntryByType(nodeType);
                        if (nodeTypeEntry == null)
                            throw new NotImplementedException("nodeType " + nodeType + " is not implemented");
                        break;
                }

                if (nodeTypeEntry == null && !startNode)
                    continue;

                XElement child = new XElement(name);
                node.Add(child);
                node = child;

                if (startNode)
                    continue;
            }

            doc = new XDocument(fakeroot.Elements().First());
            Console.WriteLine(doc.ToString());
        }
    }
}
