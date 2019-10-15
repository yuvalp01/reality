using Microsoft.EntityFrameworkCore.Migrations;

namespace Nadlan.Migrations
{
    public partial class iscompleteboo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "Transactions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "Hours",
                table: "Expenses",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(int),
                oldDefaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "Hours",
                table: "Expenses",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(decimal),
                oldDefaultValue: 0m);
        }
    }
}
