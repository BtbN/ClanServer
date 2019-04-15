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

            if (input.First() != SIGNATURE)
                throw new ArgumentException("Invalid signature", "input");
            input = input.Skip(1);

            bool compressed;
            switch (input.First())
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

            byte encodingSig = input.First();
            input = input.Skip(1);

            if (input.First() != (0xFF ^ encodingSig))
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

            XElement node = doc.Root;

            while (nodeBuf.Any())
            {
                nodeBuf = nodeBuf.SkipWhile(b => b == 0);

                byte nodeType = nodeBuf.First();
                nodeBuf = nodeBuf.Skip(1);

                bool isArray = (nodeType & 64) != 0;
                nodeType = (byte)(nodeType & ~64);

                XmlTypes.Entry nodeTypeEntry = XmlTypes.GetEntryByType(nodeType);

                string name = null;
                if (nodeType != XmlTypes.NodeEnd.type && nodeType != XmlTypes.EndSection.type)
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

                bool skip = true;

                if (nodeType == XmlTypes.EndSection.type)
                    break;

                if (skip)
                    continue;
            }
        }
    }
}
