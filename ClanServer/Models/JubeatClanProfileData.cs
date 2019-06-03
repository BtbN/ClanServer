using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClanServer.Models
{
    public class JubeatClanProfileData
    {
        public int ID { get; set; }

        public JubeatProfile Profile { get; set; }
        public int ProfileID { get; set; }

        [Range(1, 4)]
        public byte Team { get; set; }

        public int Street { get; set; }

        public int Section { get; set; }

        public short HouseNo1 { get; set; }

        public short HouseNo2 { get; set; }


        public int PlayTime { get; set; }

        public int TuneCount { get; set; }

        public int ClearCount { get; set; }

        public int FcCount { get; set; }

        public int ExCount { get; set; }

        public int MatchCount { get; set; }

        public int BeatCount { get; set; }

        public int SaveCount { get; set; }

        public int SavedCount { get; set; }

        public int BonusTunePoints { get; set; }

        public bool BonusTunePlayed { get; set; }
    }
}
