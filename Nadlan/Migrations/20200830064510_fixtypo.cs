using Microsoft.EntityFrameworkCore.Migrations;

namespace Nadlan.Migrations
{
    public partial class fixtypo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PeneltyPerDay",
                table: "contracts",
                newName: "PenaltyPerDay");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PenaltyPerDay",
                table: "contracts",
                newName: "PeneltyPerDay");
        }
    }
}
