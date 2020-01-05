using Microsoft.EntityFrameworkCore.Migrations;

namespace Nadlan.Migrations
{
    public partial class fixtypo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PrequencyPerYear",
                table: "expectedTransactions",
                newName: "FrequencyPerYear");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FrequencyPerYear",
                table: "expectedTransactions",
                newName: "PrequencyPerYear");
        }
    }
}
