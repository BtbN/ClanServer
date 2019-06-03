using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClanServer.Models
{
    public class JubeatClanSettings
    {
        public int ID { get; set; }

        public JubeatProfile Profile { get; set; }
        public int ProfileID { get; set; }

        public sbyte ExpertOption { get; set; }

        public sbyte Sort { get; set; }

        public sbyte Category { get; set; }

        public sbyte Marker { get; set; }

        public sbyte Theme { get; set; }

        public sbyte RankSort { get; set; }

        public sbyte ComboDisplay { get; set; }

        public sbyte Matching { get; set; }

        public sbyte Hard { get; set; }

        public sbyte Hazard { get; set; }

        public short Title { get; set; }

        public short Parts { get; set; }

        public short EmblemBackground { get; set; }

        public short EmblemMain { get; set; }

        public short EmblemOrnament { get; set; }

        public short EmblemEffect { get; set; }

        public short EmblemBalloon { get; set; }
    }
}
