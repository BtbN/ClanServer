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

            var jbScoreEntity = modelBuilder.Entity<JubeatScore>();
            jbScoreEntity
                .HasIndex(score => new { score.ProfileID, score.MusicID, score.Seq });
            jbScoreEntity
                .HasIndex(score => new { score.ProfileID, score.MusicID });
            jbScoreEntity
                .HasIndex(score => new { score.MusicID, score.Seq });
            jbScoreEntity
                .HasIndex(score => score.ProfileID);

            var jbHighScoreEntity = modelBuilder.Entity<JubeatHighscore>();
            jbHighScoreEntity
                .HasIndex(score => new { score.ProfileID, score.MusicID, score.Seq })
                .IsUnique();
            jbHighScoreEntity
                .HasIndex(score => new { score.ProfileID, score.MusicID });
            jbHighScoreEntity
                .HasIndex(score => new { score.MusicID, score.Seq });
            jbHighScoreEntity
                .HasIndex(score => score.ProfileID);
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<JubeatClanSettings> JubeatClanSettings { get; set; }
        public DbSet<JubeatClanProfileData> JubeatClanProfileData { get; set; }
        public DbSet<JubeatProfile> JubeatProfiles { get; set; }
        public DbSet<JubeatScore> JubeatScores { get; set; }
        public DbSet<JubeatHighscore> JubeatHighscores { get; set; }
    }
}
