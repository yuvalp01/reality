using Microsoft.EntityFrameworkCore.Migrations;

namespace Nadlan.Migrations
{
    public partial class fkExpected : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ExpectedTransactions_AccountId",
                table: "ExpectedTransactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpectedTransactions_ApartmentId",
                table: "ExpectedTransactions",
                column: "ApartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpectedTransactions_Accounts_AccountId",
                table: "ExpectedTransactions",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpectedTransactions_Apartments_ApartmentId",
                table: "ExpectedTransactions",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpectedTransactions_Accounts_AccountId",
                table: "ExpectedTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpectedTransactions_Apartments_ApartmentId",
                table: "ExpectedTransactions");

            migrationBuilder.DropIndex(
                name: "IX_ExpectedTransactions_AccountId",
                table: "ExpectedTransactions");

            migrationBuilder.DropIndex(
                name: "IX_ExpectedTransactions_ApartmentId",
                table: "ExpectedTransactions");
        }
    }
}
