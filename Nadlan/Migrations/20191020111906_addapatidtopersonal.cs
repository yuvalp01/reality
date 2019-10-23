using Microsoft.EntityFrameworkCore.Migrations;

namespace Nadlan.Migrations
{
    public partial class addapatidtopersonal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApartmentId",
                table: "PersonalTransactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransactionType",
                table: "PersonalTransactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalTransactions_ApartmentId",
                table: "PersonalTransactions",
                column: "ApartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalTransactions_Apartments_ApartmentId",
                table: "PersonalTransactions",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalTransactions_Apartments_ApartmentId",
                table: "PersonalTransactions");

            migrationBuilder.DropIndex(
                name: "IX_PersonalTransactions_ApartmentId",
                table: "PersonalTransactions");

            migrationBuilder.DropColumn(
                name: "ApartmentId",
                table: "PersonalTransactions");

            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "PersonalTransactions");
        }
    }
}
