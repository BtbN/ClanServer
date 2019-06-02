using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using ClanServer.Models;

namespace ClanServer
{
    public class ClanServerContext : DbContext
    {
        public ClanServerContext(DbContextOptions<ClanServerContext> options)
            : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cardEntity = modelBuilder.Entity<Card>();
            cardEntity
                .HasIndex(card => card.CardId)
                .IsUnique();
            cardEntity
                .HasIndex(card => card.RefId);
            cardEntity
                .HasIndex(card => card.DataId);

            var juProfEntity = modelBuilder.Entity<JubeatProfile>();
            juProfEntity
                .HasIndex(profile => profile.JID)
                .IsUnique();
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Card> Cards { get; set; }
        
        public DbSet<JubeatProfile> JubeatProfiles { get; set; }
    }
}
