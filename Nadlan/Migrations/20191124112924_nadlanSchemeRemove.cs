using Microsoft.EntityFrameworkCore.Migrations;

namespace Nadlan.Migrations
{
    public partial class nadlanSchemeRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "transactions",
                schema: "nadlan",
                newName: "transactions");

            migrationBuilder.RenameTable(
                name: "stakeholders",
                schema: "nadlan",
                newName: "stakeholders");

            migrationBuilder.RenameTable(
                name: "portfolios",
                schema: "nadlan",
                newName: "portfolios");

            migrationBuilder.RenameTable(
                name: "personalTransactions",
                schema: "nadlan",
                newName: "personalTransactions");

            migrationBuilder.RenameTable(
                name: "expenses",
                schema: "nadlan",
                newName: "expenses");

            migrationBuilder.RenameTable(
                name: "expectedTransactions",
                schema: "nadlan",
                newName: "expectedTransactions");

            migrationBuilder.RenameTable(
                name: "apartments",
                schema: "nadlan",
                newName: "apartments");

            migrationBuilder.RenameTable(
                name: "accounts",
                schema: "nadlan",
                newName: "accounts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "transactions",
                newName: "transactions",
                newSchema: "nadlan");

            migrationBuilder.RenameTable(
                name: "stakeholders",
                newName: "stakeholders",
                newSchema: "nadlan");

            migrationBuilder.RenameTable(
                name: "portfolios",
                newName: "portfolios",
                newSchema: "nadlan");

            migrationBuilder.RenameTable(
                name: "personalTransactions",
                newName: "personalTransactions",
                newSchema: "nadlan");

            migrationBuilder.RenameTable(
                name: "expenses",
                newName: "expenses",
                newSchema: "nadlan");

            migrationBuilder.RenameTable(
                name: "expectedTransactions",
                newName: "expectedTransactions",
                newSchema: "nadlan");

            migrationBuilder.RenameTable(
                name: "apartments",
                newName: "apartments",
                newSchema: "nadlan");

            migrationBuilder.RenameTable(
                name: "accounts",
                newName: "accounts",
                newSchema: "nadlan");
        }
    }
}
