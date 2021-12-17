using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballLeague.DataAccess.Migrations
{
    public partial class RenameTeamScoreToTeamPoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TeamScore",
                table: "Teams",
                newName: "TeamPoints");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TeamPoints",
                table: "Teams",
                newName: "TeamScore");
        }
    }
}
