using Microsoft.EntityFrameworkCore.Migrations;

namespace Nadlan.Migrations
{
    public partial class defaultvalcost2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                schema: "renovation",
                table: "Products",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                schema: "renovation",
                table: "Products",
                nullable: false,
                oldClrType: typeof(decimal),
                oldDefaultValue: 0m);
        }
    }
}
