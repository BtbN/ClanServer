using System;
using System.IO;
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
            string eamuse_info = "1-5cf9445d-0dfe";
            byte[] data = HexToBytes("93b01743b29ca06e7500db42d83c70843dc776d0617ac96ba0768dd6b457554591d4b8b5f963e12d5fbb5075684c2a9acbfc462aa52686a720c57b3e44373008178684f9fd7ddad3c3a1e9fe1422ae08b9b872520a64cc195a9c04585149ec8de30220345c023663ae916068117ab7d5619362019d18a6f789bbd27e4ee027ce236d2b8d6c0f0917c8990083b741b3958cdf770f970df13088f931da949d1c9f685ba7848a15c3b77083357a6fb430b8a914bf55249f092c2baf14adfa8a7ab6bd430cc6ca5b4a35ea8c893aaa0c88ae6305240d5ae479976caf35e29d943ec628752c191ae40d0998c28e3280b6a55f8198ae");

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
