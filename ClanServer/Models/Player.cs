using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClanServer.Models
{
    public class Player
    {
        public int ID { get; set; }

        public ICollection<Card> Cards { get; set; }

        public JubeatProfile JubeatProfile { get; set; }
    }
}
