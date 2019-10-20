using Microsoft.EntityFrameworkCore.Migrations;

namespace Nadlan.Migrations
{
    public partial class changeacountidtostakeholderid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Portfolios_Accounts_AccountId",
                table: "Portfolios");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Portfolios",
                newName: "StakeholderId");

            migrationBuilder.RenameIndex(
                name: "IX_Portfolios_AccountId",
                table: "Portfolios",
                newName: "IX_Portfolios_StakeholderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolios_Stakeholders_StakeholderId",
                table: "Portfolios",
                column: "StakeholderId",
                principalTable: "Stakeholders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Portfolios_Stakeholders_StakeholderId",
                table: "Portfolios");

            migrationBuilder.RenameColumn(
                name: "StakeholderId",
                table: "Portfolios",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Portfolios_StakeholderId",
                table: "Portfolios",
                newName: "IX_Portfolios_AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolios_Accounts_AccountId",
                table: "Portfolios",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
