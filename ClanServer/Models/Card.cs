using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using ClanServer.Helpers;

namespace ClanServer.Models
{
    public class Card
    {
        public int ID { get; set; }

        public Player Player { get; set; }
        public int PlayerID { get; set; }

        [Required]
        [MaxLength(16), MinLength(16)]
        public string CardId { get; set; }

        [Required]
        [MaxLength(16), MinLength(16)]
        public string DataId { get; set; }

        [Required]
        [MaxLength(16), MinLength(16)]
        public string RefId { get; set; }
    }
}