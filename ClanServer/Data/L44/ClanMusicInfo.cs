using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

using eAmuseCore.KBinXML;
using eAmuseCore.Compression;

namespace ClanServer.Data.L44
{
    public class ClanMusicInfo
    {
        static ClanMusicInfo()
        {
            Instance = Task.Run(() =>
            {
                foreach (string name in Assembly.GetExecutingAssembly().GetManifestResourceNames())
                {
                    if (name.EndsWith("music_info_l44_8.kbin", StringComparison.InvariantCultureIgnoreCase))
                    {
                        using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
                        using (MemoryStream ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            return new ClanMusicInfo(
                                new KBinXML(
                                    LZ77.Decompress(
                                        ms.ToArray()
                                    ).ToArray()
                                ).Document
                            );
                        }
                    }
                }

                return null;
            });
        }

        public static Task<ClanMusicInfo> Instance { get; private set; }

        private readonly XDocument doc;

        private ClanMusicInfo(XDocument doc)
        {
            this.doc = doc;

            BuildMusicList();
        }

        private List<int> musicIdList;
        public IList<int> MusicIdList { get => musicIdList.AsReadOnly(); }

        private void BuildMusicList()
        {
            XElement body = doc.Root.Element("music_data").Element("body");
            var musicData = body.Elements("data");

            musicIdList = new List<int>(musicData.Count());

            foreach (XElement musicEntry in musicData)
            {
                musicIdList.Add(int.Parse(musicEntry.Element("music_id").Value));
            }
        }

        private static readonly Random rng = new Random();

        public List<int> GetRandomSongs(int num)
        {
            HashSet<int> candidateIndexes = new HashSet<int>();
            while (candidateIndexes.Count < num)
                candidateIndexes.Add(rng.Next(musicIdList.Count));

            List<int> result = candidateIndexes.Select(i => musicIdList[i]).ToList();

            for (int i = result.Count - 1; i > 0; --i)
            {
                int k = rng.Next(i + 1);
                int v = result[i];
                result[i] = result[k];
                result[k] = v;
            }

            return result;
        }
    }
}
