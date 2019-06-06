using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClanServer
{
    public static class CacheKeys
    {
        public static string GetRecommendedSongsKey(int jid) => $"_RecommendedSongs_{jid}";
    }
}
