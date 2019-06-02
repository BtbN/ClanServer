using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClanServer.Models
{
    public class JubeatProfile
    {
        public int ID { get; set; }

        [Required]
        public int JID { get; set; }

        [Required]
        public string Name { get; set; }

        public JubeatClanProfileData ClanData { get; set; }

        public JubeatClanSettings ClanSettings { get; set; }
    }
}
