using System;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;

using eAmuseCore.Crypto;
using eAmuseCore.Compression;
using eAmuseCore.KBinXML;

namespace eAmuseTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string compress = "lz77";
            string eamuse_info = "1-5ca3982d-98f8";
            byte[] data = HexToBytes("c119c325f093ef6c5f46eaafcd159e0f93648a37b415820de167b9b7f49dfc38736cf807ae57bc56e01f0a8e4ed9de1df774d854f9a1a751d8ef04cbdd4c951d2c1ac578f1ae7fe55e73ecba6f0f7aca4505adaf15a0586e3f28550e59390aea052cb25c1bceafca22f253f6b8ee77f9f69c42e0e7c8eb4f736376372e73c04d5c337f79c4b39804f19024eb850d94cbec79da6158bcc3a23bc67124bcd2bc1ac32c9162c550eb9ec0b1299bcb081325b18a91d691cdd9e190a2ae4d70764ecf9579901d7063efe9b657d83e0703de228c186b0fa85d973e98d9d4c2dfe6827a47ee9a8bc145454c0ecf673d82a2a84d0c2a1d061232ed979e143b1ecf1a1c55e43225a20cf4b212cdb07f138e76fb8e023ebb0be62038483c2a4c7b7319b03d23bf673f3feb0d987e54c3e50f5532e96bfca5434027489750954ad8a6943fa12ec4b87df6608d455411bbdce01b06606843b3c48e6f2cd8857c7f4530db2028691fe4ce7b496cbd1fbf5eb65953899b143217bc927c126d0ba53585ab00e2c561df9fbae915b7dfdf66eb8e18254ae38acd2d68889494a6ffe2e00c1aa2c4284a410c217297c263ba2af8c8e8f09bf3931afe0c012c6da39f3e2bf223fda7b53907393b38b7ae4d28422cad61a9a73514aeeb2dddf00a18ff9b0733cb7820f0d2b29467494d539a0696cfeee359bc9625c75aac059d7ca919c15674a234fe971852a163faad3169364c52992caf47c2db673b604b0727480c436fee525b2c7a782a03b8c7f2004c280fc186fb6a5d8ef4b48d417cf79f04c4878b0c254eb86b7954ec69c76ed1979f6f055620e85b60a97494e0ceb6e382054e7827cd3e05488e46e32341bbe652c855394a6ca109b856bab16417405057b32b0cb4361abb6f95b981ded651e31736bed382313438a26ad370029ca9188e447645ddecbf550387896959cb2d701720e5dfedac5750568da387c90afed4ce71e25a37af674ce86f83df1972a65eaf68f3c7ed8b1a522fb4ab61ad3db44250d000b79137bf703624b9e92d8fb67ad04dffdac2f4a016418caa5b7201820c0697437ad52249a4d8cfb604c0f50c217f30d03ece570e8e1c74be7ed3ea48a0018dd98bd48b388be9959995b185072db1f5a27d1b76362c00335bc44d8285baf90b1db9cb59a3c3814857837766974fd1d3c936da97f084d74c7efe287d53a1c168bc1dc14659b872e44c3864156f476b41251f927335eae425fbc5412ef5454fbe9c83ed701bb436b6e255db6f193894afcf8477c0053de5a1f0653f452ca53fda06cfaa73cf269e65be05eb1c2f68f152a446fae871ee185b73846077325454c575990a1640c1f96e005ae9698faa8c7f93091892c9a30260f05350e391e738bd65c9a76636de9fc0871a1433d2c574b2375bf46f7b5df94be57fd38223eb088b2b2537da0c50bec23c865227e67f16d1064cd58b36e03b13c3bba532458d81011ffa48e3ec3b22746fc6618c3b9dac69529c2f6070be0ead0c0a3a57a7c51b04c5e975141989f6a9ed45212d662a7be6e061927c70a4d9a09c040492c40ca32630bde78e3ea6e23ad848b6b474c2f014ba92630bcd31ab360eed244128db872011132160113e75f343af106c3f250933815e54dfb5c682f45209b6ad008dff0c4ed14e2ef4df8548b0d51b9b85721e3e8b6da7cff7c4b76c81a2d9f8485ad64ab4d5e5595646c16805b701c70240d7df7caa2bc0d932bb14b936445bb11fcc0a2d09fdb61b2d0fdf9261ae1f179c8f44621f372ba239ccfc3319249a8741ddf4337a2a040da64f0b61a0c6ab1fac5156f9c9e9c7b7d487933e8fbda0fc02d213e88aa92d357d9878f35fed46e7d6837ef789e1c32f839a684f8954560b13de7d98d4de2efd2034020799da8964b9602872192620e84f0df4dede10c2ea67fe1a4899868f8fce3a585f4936a72557c82db3943874a8f117c9c1f62fa5fdd1f8aa177828b630cca3f2f7f86451c19c99a49dc3cd60f770e650cf60379443d72d44889df15b223b94b44128c9e3155b7fd0eb1442e67396cf39bc35b89c3717659f05eb2c87f970cdac0ce19eaeb9e96ecf19a74cda7a2130a225c7b369d33f77b71d179467c70afda013f6e504f4b27af5f739b3aabd34edb79c9de365bbf9cc28fad4352d35c6e3c108935ffcee1b4a6670878dc9abe82716fc06834f21b3f2f9d9368622229b9152e0b36d60e41b72cc6b9a05ba508d9c69c90e884b796eb9d007fc8a781a28dd5b5c3285772d1642fe29126d468b32a9a9bbdd1926033032366ea3b650ca17766863d7642316b5b00297f82466e23574d391daee340183400452070e79c66ebdf88075d33c103a3a6614056159fdceff6b69467ab2af49425e62df863af55a095690eca2b9bd860e0b249fd954e878ff40facf35131e5a6dec2eaae8460d63d02b8a48466bfd2897a47545ce5ba24f40b93e17cd2f5b90d139fb329dcf07e9631a9123285c8099dade62d2a7d1bcbd362994b813c0d6816e2f9ea3c1359e23b5bceefe428336c2f0cddd2907c97c308f4660b9250b1d67da0c2a26e9ac2dc9b5e5439fda31fbf3ccb95ff773a6c041e0e8608accb4a612d17a06643b17e2af07de0e31a23f7e7421640f1e6f7627417e1d0aad7758ebe37bd979b43bd672c40487ed58db816b96a36edf9a323d76ab67e97346457d26977eb3443ec8c39b2faada8df3b97904636c3ea38ade93732ed4777d9889645baa016e6ff2b712f4a07a3724d85dc89b877325302651ea78f0aaa18da3e5579b682aca01764170034c2c3e08c61222c725ece83a712d98223d16a45a19507258a59ac64cf474750a5328e91e9364deb105ddbd193f8dde6f70d60349a4b30c0c9879f2476e6e1a542a3f276f2c9f4d922e303e0a38483249cd7480f2905dd77116aee6d426d34386437a23c02de911c356f155f1091bda37c15fd0e7d9539dbd928cb3c7e4e1403a4aefcd9bf0dffbe993d3f9ff369fdc0e90b89f6082e96e504d469b9329cca07c9d13b2ec8d09d743089af2263a0597f67a3d71e4fec1d8ab1fb23448a45863d9e122b8873a18abf9316617c82d418b13d6cf5ba2ff94f2dff602142e6ee241167d20e42d389fd50cab3e664a67b1448e62f2fac5f34633b85e79d0b98267f689b58ee78653394065f1d3bda945c90d4a66b84d61fe43f31f297c9e4ea177ee4be9139ca823bba69f2a7875ae55b0d8da798893c4bcd25f794da5ba99649283f8158618bb6cee794cf12d71a2a29e1171229bcd6ddd85baeba5d1b6fcfecbbe2ad3e19854c2afc73002ebc60c11833d9261b46044e53aef0daa50");

            compress = compress.ToLower();

            var decryptedData = RC4.ApplyEAmuseInfo(eamuse_info, data);
            var rawData = decryptedData;
            if (compress == "lz77")
                rawData = LZ77.Decompress(decryptedData);
            else if (compress != "none")
                throw new ArgumentException("Unsupported compression algorithm");

            Console.WriteLine("GO");

            KBinXML kbinxml = new KBinXML(rawData.ToArray());

            Console.WriteLine(kbinxml);
        }

        private static string BytesToString(IEnumerable<byte> bytes)
        {
            var sb = new StringBuilder();

            foreach (byte b in bytes) {
                char c = (char)b;
                if (c == '\n')
                    sb.Append("\\n");
                else if (c == '\r')
                    sb.Append("\\r");
                else if (c == '\t')
                    sb.Append("\\t");
                else if (b < 128 && (char.IsLetterOrDigit(c) || char.IsPunctuation(c) || char.IsWhiteSpace(c)))
                    sb.Append(Convert.ToString(c));
                else
                    sb.Append("\\x" + Convert.ToString(b, 16).PadLeft(2, '0'));
            }

            return sb.ToString();
        }

        private static byte[] ExtractResource(string fileName)
        {
            var a = System.Reflection.Assembly.GetExecutingAssembly();
            using (var fStream = a.GetManifestResourceStream(fileName))
            {
                if (fStream == null)
                    throw new ArgumentException("resource does not exist", "fileName");
                byte[] res = new byte[fStream.Length];
                for (int pos = 0; pos < res.Length; )
                    pos += fStream.Read(res, pos, res.Length - pos);
                return res;
            }
        }

        public static byte[] HexToBytes(string hex)
        {
            int len = hex.Length;
            byte[] bytes = new byte[len / 2];
            for (int i = 0; i < len; i += 2)
                bytes[i >> 1] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}
