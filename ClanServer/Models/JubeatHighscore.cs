using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClanServer.Models
{
    public class JubeatHighscore
    {
        public int ID { get; set; }

        [Required]
        public JubeatProfile Profile { get; set; }
        public int ProfileID { get; set; }

        public long Timestamp { get; set; }

        public int MusicID { get; set; }

        [Range(0, 2)]
        public sbyte Seq { get; set; }

        public int Score { get; set; }

        public sbyte Clear { get; set; }

        public int PlayCount { get; set; }

        public int ClearCount { get; set; }

        public int FcCount { get; set; }

        public int ExcCount { get; set; }

        [MinLength(30), MaxLength(30)]
        public byte[] Bar { get; set; }
    }
}
