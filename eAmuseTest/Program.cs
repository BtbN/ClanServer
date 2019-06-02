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
        static void Main()
        {
            string compress = "lz77";
            string eamuse_info = "1-5cf415a4-5963";
            byte[] data = HexToBytes("5859dc2ca4b1e49d229d834affa7922f65ba9d578bc1ad8f885543a58ededfcbd861f2955d509ec8c71d1e96731431b1c19c73d80febb611e7d1a3cfe67435afefae2236865187c84c7e365fa80b721349c7ff2b13e87f9ff54d6fe784ece51d14bdd30b3c5c7da6848ef523321c281840e7fae20674cfd8763308720f64755ba51c1c2535f134c5a9ae60413b660362c685be9232ed7171d42c06c3a25cef442f647f14f02ee5d064aeef4bf2a34659bb095760880c7b54da74141f707facb827447f3d23d8ea167fff5a4d22e393f8d63f1f277254d3f3b295450e53121f");

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

            Console.WriteLine(kbinxml.ToString());

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
