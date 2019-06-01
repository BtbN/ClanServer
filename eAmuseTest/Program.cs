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
            string eamuse_info = "1-5cf04a26-3fad";
            byte[] data = HexToBytes("192b636607070f377148b5595cd163d2f224970b04a6aceae90c1885895c265707be27cdb266822bac0a49a9ccdc67218b34e5f52efcd797bc0ac5e2bee85e252b03e152865c6f0fb9e9bfa31c8f76feef59cb00fb6fb2e33f265d91adab4cbb49221b4c3453047cb7ca865cfd904417b526ff8b0310b5a64e3afc303ed3c756d36fd341e5541025ee0d740da97b3e739aaf1e49520fb39d860c6e43c42eab9b31523733942a81d671468301d5f3cb9b2e15ea8de4151ccc6414afb4ed062437257f082b2a446f089b9fa40c4fd9c60c80b612c3990f4002cf983b29616e32f7a545de443531fa0570fed58106a4bb77194f8bea18d347165caabe5332a12fe69e78e91c8b18b02eab9f205cc31021ae63270398e9610e04297b709dacf3310c14df753d8affae0d3f1b6712f8d663b7262a8d351e8184e5cfcf1fb2ac83a4ac2eefb918cae357b3013ea3a45b4906f7e5dbdf0eb9cdaced18acd5836e4ec05cc221b68217f2cd7abe90a0b70e3a7bb19c3dd54c623f554c3ce409c194d6fe104c1dbea3e9e61726336b59c1d27b7fd05f746b767d28209a4dbe432e06d3322fcd3dd92fe7dc950d9d2de6a54999938cbe76f483751b69d969e7cbf20179b91fb97fe5fe285ecdea9b754f649305909afa56b0c85c5335cc805cd1cfce2ac5656da1b46f9a50147dfe1d9d0e98f17c149daba41d9628bd77adad0e9c3e7c8b8ee3f6c1e112f3b770dfe361626107264a618b5db4fc70bd46c147031ccbe07ec7face27d5473f4bc414db4b1d37e913b2c31eac8e3a7819566f7b297495422e77cc7a6a3a9a5bd35d290ea2c32bedde889110400a668312c81b5958c8c2c65797e6cd2bc1e28d70be10e7e873dd5ecea371eba78f13bddf913d37465a5b3b81be08a5e911d34b4801838073b2bd8e53c0c0f559f3b9b39bbc663f9f1b7b907ffc436f3d35517d9c297c0a9a7ed7e492cbce674df575d22025e3b80d630d6fe3142c2b72adbb896b9b08b86ace6af4f38655c18894ccc6ea0ae6bcea31e01b70a642b65e530e5e3cf0d479f6b07faede5c545c139203f6abae96957df6899d7f9a638bb52bb20a53e804218ac031287c415b28de6e1d49b3875125212cd4090915ef1fc88dead3dfa77fff6b4c9fcc93148cd674f84e1fb49d91288b80c298691fdeb98a832fa85b1da2a9a067640849bdef235b2af8baa7ee3e31983e3327a0d4550c72fdf1722e72b98c2431c7f58ca2e76814faee980eaf22f0ba73c6a199d69d36261f118255ccc6e13041a82ad98837c12634c1e0ce5b8461c7e326bbdec46c1f3d2165be3a22a05c2f30be704337f45c455613800a14e333c1dd2a31a4b344e395cbe57a75a340babd40ebbd9e60008a62956c3eb3add3d53891243ca7c4875651b7a5cac07336c55f37548c3576243af80762334af9c66458bfce1946969ca5fdfa4944f957cb5ad029442624b35fa1783b613247ec2c292cc9b59c2ba767b6e083b813f80bbe2120891956f6235b55811af6aae6e80759805ed73e6e1c43c8d8fcefaf844473d7e3f329ab62a705fc04fb68561f76c23fd2ad1bd1c0fd5e8e3ea68b42ee4d220833fe7e488bfc87319decbe512857a55e7505a5c3760d0623ae170ba541afe9cb7880a82607be87c91eefb95ce67cadd30816320641c923a0431aa69d897bf017cb11712170e3d3425de7357fd7c5970dd0611e0645bc0b19489236e00db3a85bcc13846f2432074a373d4766bf88f0eb1e349c63f98d160aafa29ffc2c6da89c3e4036a5573c8883c53f62d83e76a7b0412a1868230cc16c78e24f61e249cd0f66f05a2d976fc9ad5f0d19013866236c8db928b18ccc2e42c27942f3b03a2bdd98160c7c19c06f8b47189e40c56a66482f78c0864d6637247054004c84a7f909fd78351da411d858d0403f15cf7de0b57f716a2c1c3044d1a337a4785c67f278bc82be8b246d0974cffe7305bbae18bbdbe3e1ca00699894e6fb9576cfea806be34e895eaae33f56214fda4b7c9804f5e12153ff2d81b53e7eae5d9afdd3a2d935ce62e757b9afc7086bb44c204e1bc9d9d6e39600bebf3e7f3fd21e825f94dc30cf4e3c73bdf7ea21ba75fd38cc98121d287b5983ec591f06979c875a653923ba34992559b1773e58de0862d1685cf91993511c312b9e16a0e8ec64f456c49c13d9eb0cf4b31c754318a17de0c162e41e79afa0a05dd79c98f61db2d8d500894f0ced55a816a7da3b9ea4361e7912a90258bc0f2b2fa2cbf7e85b70ae067b78291830d1343a8bf91e0127650cdd67d18359a53aed1b71b3adfdb0fbefc3c3fdb133f4241581182a86b4f6783d328c1f4be0180e350704c5ec808ec44322816e084cda96632d4d073afff921f76e1c166716f6a7de2ba5cc555a69561007d96e1fb7acf1b4e6e4a0c979818dde562a17d3d49f6a4a503892f5e04b543c91bdb2150df3fc2eb1860e4643217ebfdab57aff73de05a546ee653eaec1216ed1e8b73f50e2a2a84c1f0437ed16f7ea557a6c32ba46db79edce000b211d5ab0166e82085fda2881d5e22e62131a031c93158a78a4118f5d28d3ce4a0ca9747dac83e0ed2ae1f594b83af110524e252c0dc331f06a2b608396878fe92e531fd76bfb590c202bd2e9a899b44d716e459329b4b6791ff79dfd8b2303ec4e9c0af4bc022727589795d6f7e4c633994824724f2783dbc74b4c79430009505c7c7ffd7c74c361a857aa336776c66d06fde104f68c9931ae2df44eb71280cc8f46a8d546c6c768f3bea7f395c9dc3f28645f72965ae73d0639385e26d2240bcb2f61bb016b9013a4533c32811081c93f88a05e6824edb19b9568ee9693dab036b7ea68693d584455afa0d6169f4e634d848468a2fd2310b0a377de58a2694bc38cb6c9de0ca5eada717ed623db1f637f5a7a67d8642eae3dd6bc605813f2e76c811da394df950d55b40eba83b12b90dd6b5b487fe11bf0ff882a024a85051657c81dbff31dd7bc79da0a7181449c452e67ce6cf012f0e30e37264f034aecc33db1e356371f901439fc2c8d69b3ee0388cbd0ab83692f9a32e113403de48bb10741aa485eca3392a09b0fb64fc7ecd4ab408fd05bd48aa58852f111df8e508e7945ed23c41bb3a2db3e4f4b7f1d83f7ac429f5406b76a83452223db9694668f2b24c7b9ab7c119d64d8c022bc6cd9802868ead9a2182f1385766153c25c77bf512825c2af8aad25eea29a809df5ccc9167316acb13829cd7086ce7dbe03cca7aa4db28ed9ef14ee204c51b5a580495cbf345f731db33783cc8cddffd66f101f110963978a3804fa0fcceab61cd11a8e5c08a6c965a14ee09fd9375b20646b25a7eee4ffa11f5f8ac69d28304c71726f8e75");

            compress = compress.ToLower();

            IEnumerable<byte> decryptedData = data;
            if (eamuse_info != null)
                decryptedData = RC4.ApplyEAmuseInfo(eamuse_info, data);

            var rawData = decryptedData;
            if (compress == "lz77")
                rawData = LZ77.Decompress(decryptedData);
            else if (compress != "none")
                throw new ArgumentException("Unsupported compression algorithm");

            KBinXML kbinxml = new KBinXML(rawData.ToArray());

            Console.WriteLine(kbinxml);

            //GenerateEchidnaSQL(kbinxml);
        }

        private static void GenerateEchidnaSQL(KBinXML get_mdata_data)
        {
            const int profile_id = 1;

            XDocument doc = get_mdata_data.Document;
            XElement mdata_list = doc.Element("response").Element("gametop").Element("data").Element("player").Element("mdata_list");

            foreach (XElement music in mdata_list.Elements("music"))
            {
                long music_id = long.Parse(music.Attribute("music_id").Value);

                for (int seq = 0; seq <= 2; ++seq)
                {
                    int score = int.Parse(music.Element("score").Value.Split(' ')[seq]);
                    if (score <= 0)
                        continue;

                    int clear = int.Parse(music.Element("clear").Value.Split(' ')[seq]);
                    int play_cnt = int.Parse(music.Element("play_cnt").Value.Split(' ')[seq]);
                    int clear_cnt = int.Parse(music.Element("clear_cnt").Value.Split(' ')[seq]);
                    int fc_cnt = int.Parse(music.Element("fc_cnt").Value.Split(' ')[seq]);
                    int ex_cnt = int.Parse(music.Element("ex_cnt").Value.Split(' ')[seq]);

                    string barData = "";
                    foreach (XElement bar in music.Elements("bar"))
                    {
                        if (bar.Attribute("seq").Value != seq.ToString())
                            continue;

                        foreach (string v in bar.Value.Split(' '))
                            barData += byte.Parse(v).ToString("X2");
                    }

                    long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() * 1000L;

                    string query = $@"
INSERT INTO jubeat_scores(
    music_id,
    timestamp,
    score,
    clear_type,
    seq,
    num_perfect,
    num_great,
    num_good,
    num_poor,
    num_miss,
    play_count,
    clear_count,
    fc_count,
    ex_count,
    bar,
    music_rate,
    is_hard_mode,
    is_hazard_end,
    best_score,
    best_music_rate,
    best_clear,
    profile_id
) VALUES (
    {music_id},
    {timestamp},
    {score},
    {clear},
    {seq},
    0,
    0,
    0,
    0,
    0,
    {play_cnt},
    {clear_cnt},
    {fc_cnt},
    {ex_cnt},
    X'{barData}',
    -1,
    0,
    0,
    {score},
    -1,
    {clear},
    {profile_id}
);";

                    Console.WriteLine(query.Replace("\n", "").Replace("\r", "").Replace("    ", " "));
                }
            }
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
