using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClanServer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Passwd = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerID = table.Column<int>(nullable: false),
                    CardId = table.Column<string>(maxLength: 16, nullable: false),
                    DataId = table.Column<string>(maxLength: 16, nullable: false),
                    RefId = table.Column<string>(maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Cards_Players_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "Players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JubeatProfiles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JubeatProfiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_JubeatProfiles_Players_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "Players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JubeatClanJubility",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProfileID = table.Column<int>(nullable: false),
                    MusicID = table.Column<int>(nullable: false),
                    Seq = table.Column<sbyte>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    Value = table.Column<int>(nullable: false),
                    IsHardMode = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JubeatClanJubility", x => x.ID);
                    table.ForeignKey(
                        name: "FK_JubeatClanJubility_JubeatProfiles_ProfileID",
                        column: x => x.ProfileID,
                        principalTable: "JubeatProfiles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JubeatClanProfileData",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProfileID = table.Column<int>(nullable: false),
                    Team = table.Column<byte>(nullable: false),
                    Street = table.Column<int>(nullable: false),
                    Section = table.Column<int>(nullable: false),
                    HouseNo1 = table.Column<short>(nullable: false),
                    HouseNo2 = table.Column<short>(nullable: false),
                    PlayTime = table.Column<int>(nullable: false),
                    TuneCount = table.Column<int>(nullable: false),
                    ClearCount = table.Column<int>(nullable: false),
                    FcCount = table.Column<int>(nullable: false),
                    ExCount = table.Column<int>(nullable: false),
                    MatchCount = table.Column<int>(nullable: false),
                    BeatCount = table.Column<int>(nullable: false),
                    SaveCount = table.Column<int>(nullable: false),
                    SavedCount = table.Column<int>(nullable: false),
                    BonusTunePoints = table.Column<int>(nullable: false),
                    BonusTunePlayed = table.Column<bool>(nullable: false),
                    JubilityParam = table.Column<int>(nullable: false),
                    JboxPoints = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JubeatClanProfileData", x => x.ID);
                    table.ForeignKey(
                        name: "FK_JubeatClanProfileData_JubeatProfiles_ProfileID",
                        column: x => x.ProfileID,
                        principalTable: "JubeatProfiles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JubeatClanSettings",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProfileID = table.Column<int>(nullable: false),
                    ExpertOption = table.Column<sbyte>(nullable: false),
                    Sort = table.Column<sbyte>(nullable: false),
                    Category = table.Column<sbyte>(nullable: false),
                    Marker = table.Column<sbyte>(nullable: false),
                    Theme = table.Column<sbyte>(nullable: false),
                    RankSort = table.Column<sbyte>(nullable: false),
                    ComboDisplay = table.Column<sbyte>(nullable: false),
                    Matching = table.Column<sbyte>(nullable: false),
                    Hard = table.Column<sbyte>(nullable: false),
                    Hazard = table.Column<sbyte>(nullable: false),
                    Title = table.Column<short>(nullable: false),
                    Parts = table.Column<short>(nullable: false),
                    EmblemBackground = table.Column<short>(nullable: false),
                    EmblemMain = table.Column<short>(nullable: false),
                    EmblemOrnament = table.Column<short>(nullable: false),
                    EmblemEffect = table.Column<short>(nullable: false),
                    EmblemBalloon = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JubeatClanSettings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_JubeatClanSettings_JubeatProfiles_ProfileID",
                        column: x => x.ProfileID,
                        principalTable: "JubeatProfiles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JubeatHighscores",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProfileID = table.Column<int>(nullable: false),
                    Timestamp = table.Column<long>(nullable: false),
                    MusicID = table.Column<int>(nullable: false),
                    Seq = table.Column<sbyte>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    Clear = table.Column<sbyte>(nullable: false),
                    PlayCount = table.Column<int>(nullable: false),
                    ClearCount = table.Column<int>(nullable: false),
                    FcCount = table.Column<int>(nullable: false),
                    ExcCount = table.Column<int>(nullable: false),
                    Bar = table.Column<byte[]>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JubeatHighscores", x => x.ID);
                    table.ForeignKey(
                        name: "FK_JubeatHighscores_JubeatProfiles_ProfileID",
                        column: x => x.ProfileID,
                        principalTable: "JubeatProfiles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JubeatScores",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProfileID = table.Column<int>(nullable: false),
                    Timestamp = table.Column<long>(nullable: false),
                    MusicID = table.Column<int>(nullable: false),
                    Seq = table.Column<sbyte>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    Clear = table.Column<sbyte>(nullable: false),
                    NumPerfect = table.Column<short>(nullable: false),
                    NumGreat = table.Column<short>(nullable: false),
                    NumGood = table.Column<short>(nullable: false),
                    NumPoor = table.Column<short>(nullable: false),
                    NumMiss = table.Column<short>(nullable: false),
                    IsHardMode = table.Column<bool>(nullable: false),
                    IsHazardMode = table.Column<bool>(nullable: false),
                    Bar = table.Column<byte[]>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JubeatScores", x => x.ID);
                    table.ForeignKey(
                        name: "FK_JubeatScores_JubeatProfiles_ProfileID",
                        column: x => x.ProfileID,
                        principalTable: "JubeatProfiles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardId",
                table: "Cards",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_DataId",
                table: "Cards",
                column: "DataId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_PlayerID",
                table: "Cards",
                column: "PlayerID");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_RefId",
                table: "Cards",
                column: "RefId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JubeatClanJubility_ProfileID",
                table: "JubeatClanJubility",
                column: "ProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_JubeatClanProfileData_ProfileID",
                table: "JubeatClanProfileData",
                column: "ProfileID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JubeatClanSettings_ProfileID",
                table: "JubeatClanSettings",
                column: "ProfileID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JubeatHighscores_ProfileID",
                table: "JubeatHighscores",
                column: "ProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_JubeatHighscores_MusicID_Seq",
                table: "JubeatHighscores",
                columns: new[] { "MusicID", "Seq" });

            migrationBuilder.CreateIndex(
                name: "IX_JubeatHighscores_ProfileID_MusicID",
                table: "JubeatHighscores",
                columns: new[] { "ProfileID", "MusicID" });

            migrationBuilder.CreateIndex(
                name: "IX_JubeatHighscores_ProfileID_MusicID_Seq",
                table: "JubeatHighscores",
                columns: new[] { "ProfileID", "MusicID", "Seq" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JubeatProfiles_PlayerID",
                table: "JubeatProfiles",
                column: "PlayerID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JubeatScores_ProfileID",
                table: "JubeatScores",
                column: "ProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_JubeatScores_MusicID_Seq",
                table: "JubeatScores",
                columns: new[] { "MusicID", "Seq" });

            migrationBuilder.CreateIndex(
                name: "IX_JubeatScores_ProfileID_MusicID",
                table: "JubeatScores",
                columns: new[] { "ProfileID", "MusicID" });

            migrationBuilder.CreateIndex(
                name: "IX_JubeatScores_ProfileID_MusicID_Seq",
                table: "JubeatScores",
                columns: new[] { "ProfileID", "MusicID", "Seq" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "JubeatClanJubility");

            migrationBuilder.DropTable(
                name: "JubeatClanProfileData");

            migrationBuilder.DropTable(
                name: "JubeatClanSettings");

            migrationBuilder.DropTable(
                name: "JubeatHighscores");

            migrationBuilder.DropTable(
                name: "JubeatScores");

            migrationBuilder.DropTable(
                name: "JubeatProfiles");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
