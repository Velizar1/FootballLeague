using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballLeague.DataAccess.Migrations
{
    public partial class MatchesAndTeamsRowsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeamScore = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsPlayed = table.Column<bool>(type: "bit", nullable: false),
                    VisitingTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HostingTeamScore = table.Column<int>(type: "int", nullable: false),
                    VisitingTeamScore = table.Column<int>(type: "int", nullable: false),
                    HostingTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_HostingTeamId",
                        column: x => x.HostingTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Matches_Teams_VisitingTeamId",
                        column: x => x.VisitingTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HostingTeamId",
                table: "Matches",
                column: "HostingTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_VisitingTeamId",
                table: "Matches",
                column: "VisitingTeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
