using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClanServer.Models
{
    public class JubeatClanJubility
    {
        public int ID { get; set; }

        public JubeatProfile Profile { get; set; }
        public int ProfileID { get; set; }

        public int MusicID { get; set; }

        public sbyte Seq { get; set; }

        public int Score { get; set; }

        public int Value { get; set; }

        public bool IsHardMode { get; set; }
    }
}
