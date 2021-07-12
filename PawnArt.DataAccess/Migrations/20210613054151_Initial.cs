using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PawnArt.DataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pieces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false),
                    Moved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pieces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WhitePlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlackPlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Recurring = table.Column<bool>(type: "bit", nullable: false),
                    state_blackPlayerTime = table.Column<string>(type: "nvarchar(48)", nullable: true),
                    state_lastPlyTime = table.Column<string>(type: "nvarchar(48)", nullable: true),
                    state_whitePlayerTime = table.Column<string>(type: "nvarchar(48)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Players_BlackPlayerId",
                        column: x => x.BlackPlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Players_WhitePlayerId",
                        column: x => x.WhitePlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentPositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Boards_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerToPlayColor = table.Column<int>(type: "int", nullable: false),
                    BoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentPositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EnPassentTakableBlackPawnId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EnPassentTakableWhitePawnId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _50MovesRuleCounter = table.Column<int>(type: "int", nullable: false),
                    canBlackCastleKingSide = table.Column<bool>(type: "bit", nullable: false),
                    canBlackCastleQueenSide = table.Column<bool>(type: "bit", nullable: false),
                    canWhiteCastleKingSide = table.Column<bool>(type: "bit", nullable: false),
                    canWhiteCastleQueenSide = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Positions_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Positions_Boards_CurrentPositionId",
                        column: x => x.CurrentPositionId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sqaures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OccupierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Row = table.Column<int>(type: "int", nullable: false),
                    File = table.Column<int>(type: "int", nullable: false),
                    PositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sqaures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sqaures_Pieces_OccupierId",
                        column: x => x.OccupierId,
                        principalTable: "Pieces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sqaures_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boards_GameId",
                table: "Boards",
                column: "GameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_BlackPlayerId",
                table: "Games",
                column: "BlackPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_WhitePlayerId",
                table: "Games",
                column: "WhitePlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_BoardId",
                table: "Positions",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_CurrentPositionId",
                table: "Positions",
                column: "CurrentPositionId",
                unique: true,
                filter: "[CurrentPositionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_EnPassentTakableBlackPawnId",
                table: "Positions",
                column: "EnPassentTakableBlackPawnId",
                unique: true,
                filter: "[EnPassentTakableBlackPawnId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_EnPassentTakableWhitePawnId",
                table: "Positions",
                column: "EnPassentTakableWhitePawnId",
                unique: true,
                filter: "[EnPassentTakableWhitePawnId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Sqaures_OccupierId",
                table: "Sqaures",
                column: "OccupierId",
                unique: true,
                filter: "[OccupierId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Sqaures_PositionId",
                table: "Sqaures",
                column: "PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Sqaures_EnPassentTakableBlackPawnId",
                table: "Positions",
                column: "EnPassentTakableBlackPawnId",
                principalTable: "Sqaures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Sqaures_EnPassentTakableWhitePawnId",
                table: "Positions",
                column: "EnPassentTakableWhitePawnId",
                principalTable: "Sqaures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Games_GameId",
                table: "Boards");

            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Boards_BoardId",
                table: "Positions");

            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Boards_CurrentPositionId",
                table: "Positions");

            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Sqaures_EnPassentTakableBlackPawnId",
                table: "Positions");

            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Sqaures_EnPassentTakableWhitePawnId",
                table: "Positions");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Boards");

            migrationBuilder.DropTable(
                name: "Sqaures");

            migrationBuilder.DropTable(
                name: "Pieces");

            migrationBuilder.DropTable(
                name: "Positions");
        }
    }
}
