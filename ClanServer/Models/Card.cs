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
        [MaxLength(8), MinLength(8)]
        public byte[] CardId { get; set; }

        [Required]
        [MaxLength(8), MinLength(8)]
        public byte[] DataId { get; set; }

        [Required]
        [MaxLength(8), MinLength(8)]
        public byte[] RefId { get; set; }

        [NotMapped]
        public string CardIdStr
        {
            get => CardId.ToHexString();
            set => CardId = value.ToBytesFromHex();
        }

        [NotMapped]
        public string DataIdStr
        {
            get => DataId.ToHexString();
            set => DataId = value.ToBytesFromHex();
        }

        [NotMapped]
        public string RefIdStr
        {
            get => RefId.ToHexString();
            set => RefId = value.ToBytesFromHex();
        }
    }
}