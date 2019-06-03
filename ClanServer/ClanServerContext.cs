using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using ClanServer.Models;
using ClanServer.Helpers;

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
                .HasIndex(card => card.RefId)
                .IsUnique();
            cardEntity
                .HasIndex(card => card.DataId);

            var juProfEntity = modelBuilder.Entity<JubeatProfile>();
            juProfEntity
                .HasIndex(profile => profile.JID)
                .IsUnique();

            var jbScoreEntity = modelBuilder.Entity<JubeatScore>();
            jbScoreEntity
                .HasIndex(score => new { score.ProfileID, score.MusicID, score.Seq })
                .IsUnique();
            jbScoreEntity
                .HasIndex(score => new { score.ProfileID, score.MusicID });
            jbScoreEntity
                .HasIndex(score => score.ProfileID);
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<JubeatClanSettings> JubeatClanSettings { get; set; }
        public DbSet<JubeatClanProfileData> JubeatClanProfileData { get; set; }
        public DbSet<JubeatProfile> JubeatProfiles { get; set; }
        public DbSet<JubeatScore> JubeatScores { get; set; }
    }
}
