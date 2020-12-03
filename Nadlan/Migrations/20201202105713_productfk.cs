using Microsoft.EntityFrameworkCore.Migrations;

namespace Nadlan.Migrations
{
    public partial class productfk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_lines_ProductId",
                schema: "renovation",
                table: "lines",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_lines_products_ProductId",
                schema: "renovation",
                table: "lines",
                column: "ProductId",
                principalSchema: "renovation",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lines_products_ProductId",
                schema: "renovation",
                table: "lines");

            migrationBuilder.DropIndex(
                name: "IX_lines_ProductId",
                schema: "renovation",
                table: "lines");
        }
    }
}
