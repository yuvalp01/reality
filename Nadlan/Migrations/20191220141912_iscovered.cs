using Microsoft.EntityFrameworkCore.Migrations;

namespace Nadlan.Migrations
{
    public partial class iscovered : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPaidByInvestor",
                table: "transactions",
                newName: "IsCoveredByInvestor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCoveredByInvestor",
                table: "transactions",
                newName: "IsPaidByInvestor");
        }
    }
}
