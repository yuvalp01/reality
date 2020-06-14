using Microsoft.EntityFrameworkCore.Migrations;

namespace Nadlan.Migrations
{
    public partial class scheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_renovationPayments_projects_RenovationProjectId",
                table: "renovationPayments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_renovationPayments",
                table: "renovationPayments");

            migrationBuilder.RenameTable(
                name: "renovationPayments",
                newName: "payments",
                newSchema: "renovation");

            migrationBuilder.RenameIndex(
                name: "IX_renovationPayments_RenovationProjectId",
                schema: "renovation",
                table: "payments",
                newName: "IX_payments_RenovationProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_payments",
                schema: "renovation",
                table: "payments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_payments_projects_RenovationProjectId",
                schema: "renovation",
                table: "payments",
                column: "RenovationProjectId",
                principalSchema: "renovation",
                principalTable: "projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payments_projects_RenovationProjectId",
                schema: "renovation",
                table: "payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_payments",
                schema: "renovation",
                table: "payments");

            migrationBuilder.RenameTable(
                name: "payments",
                schema: "renovation",
                newName: "renovationPayments");

            migrationBuilder.RenameIndex(
                name: "IX_payments_RenovationProjectId",
                table: "renovationPayments",
                newName: "IX_renovationPayments_RenovationProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_renovationPayments",
                table: "renovationPayments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_renovationPayments_projects_RenovationProjectId",
                table: "renovationPayments",
                column: "RenovationProjectId",
                principalSchema: "renovation",
                principalTable: "projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
