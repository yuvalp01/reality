using Microsoft.EntityFrameworkCore.Migrations;

namespace Nadlan.Migrations
{
    public partial class removechecks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckIdWriten",
                schema: "renovation",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "CheckInvoiceScanned",
                schema: "renovation",
                table: "payments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CheckIdWriten",
                schema: "renovation",
                table: "payments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CheckInvoiceScanned",
                schema: "renovation",
                table: "payments",
                nullable: false,
                defaultValue: false);
        }
    }
}
