using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "DefaultBoats",
                table => new
                {
                    DefaultBoatId = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Length = table.Column<int>("int", nullable: false),
                    Name = table.Column<string>("nvarchar(32)", maxLength: 32, nullable: false),
                    CreatedDateTime = table.Column<DateTime>("datetime2", nullable: false),
                    DeletedDateTime = table.Column<DateTime>("datetime2", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_DefaultBoats", x => x.DefaultBoatId); });

            migrationBuilder.CreateTable(
                "GameOptions",
                table => new
                {
                    GameOptionId = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>("nvarchar(128)", maxLength: 128, nullable: false),
                    BoardWidth = table.Column<int>("int", nullable: false),
                    BoardHeight = table.Column<int>("int", nullable: false),
                    EBoatsCanTouch = table.Column<int>("int", nullable: false),
                    ENextMoveAfterHit = table.Column<int>("int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>("datetime2", nullable: false),
                    DeletedDateTime = table.Column<DateTime>("datetime2", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_GameOptions", x => x.GameOptionId); });

            migrationBuilder.CreateTable(
                "GameOptionBoats",
                table => new
                {
                    GameOptionBoatId = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDateTime = table.Column<DateTime>("datetime2", nullable: false),
                    DeletedDateTime = table.Column<DateTime>("datetime2", nullable: true),
                    Amount = table.Column<int>("int", nullable: false),
                    DefaultBoatId = table.Column<int>("int", nullable: false),
                    GameOptionId = table.Column<int>("int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameOptionBoats", x => x.GameOptionBoatId);
                    table.ForeignKey(
                        "FK_GameOptionBoats_DefaultBoats_DefaultBoatId",
                        x => x.DefaultBoatId,
                        "DefaultBoats",
                        "DefaultBoatId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_GameOptionBoats_GameOptions_GameOptionId",
                        x => x.GameOptionId,
                        "GameOptions",
                        "GameOptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Games",
                table => new
                {
                    GameId = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameOver = table.Column<bool>("bit", nullable: false),
                    Name = table.Column<string>("nvarchar(max)", nullable: false),
                    BoardTurnId = table.Column<int>("int", nullable: false),
                    GameOptionId = table.Column<int>("int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>("datetime2", nullable: false),
                    DeletedDateTime = table.Column<DateTime>("datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                    table.ForeignKey(
                        "FK_Games_GameOptions_GameOptionId",
                        x => x.GameOptionId,
                        "GameOptions",
                        "GameOptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "TurnSaves",
                table => new
                {
                    TurnSaveId = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CellX = table.Column<int>("int", nullable: false),
                    CellY = table.Column<int>("int", nullable: false),
                    AttackerId = table.Column<int>("int", nullable: false),
                    DefenderId = table.Column<int>("int", nullable: false),
                    GameId = table.Column<int>("int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>("datetime2", nullable: false),
                    DeletedDateTime = table.Column<DateTime>("datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TurnSaves", x => x.TurnSaveId);
                    table.ForeignKey(
                        "FK_TurnSaves_Games_GameId",
                        x => x.GameId,
                        "Games",
                        "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "GameBoards",
                table => new
                {
                    GameBoardId = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerName = table.Column<string>("nvarchar(64)", maxLength: 64, nullable: false),
                    AttackerId = table.Column<int>("int", nullable: true),
                    DefenderId = table.Column<int>("int", nullable: true),
                    BoardJson = table.Column<string>("nvarchar(max)", nullable: false),
                    GameId = table.Column<int>("int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>("datetime2", nullable: false),
                    DeletedDateTime = table.Column<DateTime>("datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameBoards", x => x.GameBoardId);
                    table.ForeignKey(
                        "FK_GameBoards_Games_GameId",
                        x => x.GameId,
                        "Games",
                        "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_GameBoards_TurnSaves_AttackerId",
                        x => x.AttackerId,
                        "TurnSaves",
                        "TurnSaveId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_GameBoards_TurnSaves_DefenderId",
                        x => x.DefenderId,
                        "TurnSaves",
                        "TurnSaveId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "GameBoats",
                table => new
                {
                    GameBoatId = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>("nvarchar(max)", nullable: false),
                    Length = table.Column<int>("int", nullable: false),
                    FacingX = table.Column<int>("int", nullable: true),
                    FacingY = table.Column<int>("int", nullable: true),
                    LocationX = table.Column<int>("int", nullable: true),
                    LocationY = table.Column<int>("int", nullable: true),
                    GameBoardId = table.Column<int>("int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>("datetime2", nullable: false),
                    DeletedDateTime = table.Column<DateTime>("datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameBoats", x => x.GameBoatId);
                    table.ForeignKey(
                        "FK_GameBoats_GameBoards_GameBoardId",
                        x => x.GameBoardId,
                        "GameBoards",
                        "GameBoardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_GameBoards_AttackerId",
                "GameBoards",
                "AttackerId",
                unique: true,
                filter: "[AttackerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                "IX_GameBoards_DefenderId",
                "GameBoards",
                "DefenderId",
                unique: true,
                filter: "[DefenderId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                "IX_GameBoards_GameId",
                "GameBoards",
                "GameId");

            migrationBuilder.CreateIndex(
                "IX_GameBoats_GameBoardId",
                "GameBoats",
                "GameBoardId");

            migrationBuilder.CreateIndex(
                "IX_GameOptionBoats_DefaultBoatId",
                "GameOptionBoats",
                "DefaultBoatId");

            migrationBuilder.CreateIndex(
                "IX_GameOptionBoats_GameOptionId",
                "GameOptionBoats",
                "GameOptionId");

            migrationBuilder.CreateIndex(
                "IX_Games_GameOptionId",
                "Games",
                "GameOptionId");

            migrationBuilder.CreateIndex(
                "IX_TurnSaves_GameId",
                "TurnSaves",
                "GameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "GameBoats");

            migrationBuilder.DropTable(
                "GameOptionBoats");

            migrationBuilder.DropTable(
                "GameBoards");

            migrationBuilder.DropTable(
                "DefaultBoats");

            migrationBuilder.DropTable(
                "TurnSaves");

            migrationBuilder.DropTable(
                "Games");

            migrationBuilder.DropTable(
                "GameOptions");
        }
    }
}