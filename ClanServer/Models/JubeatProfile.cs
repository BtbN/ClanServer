using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClanServer.Models
{
    public class JubeatProfile
    {
        public int ID { get; set; }

        public Player Player { get; set; }
        public int PlayerID { get; set; }

        [Required]
        public string Name { get; set; }

        public JubeatClanProfileData ClanData { get; set; }

        public JubeatClanSettings ClanSettings { get; set; }

        public ICollection<JubeatClanJubility> Jubilitys { get; set; }

        public ICollection<JubeatScore> Scores { get; set; }

        public ICollection<JubeatHighscore> Highscores { get; set; }
    }
}
