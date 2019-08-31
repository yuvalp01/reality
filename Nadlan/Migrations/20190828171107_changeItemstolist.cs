using Microsoft.EntityFrameworkCore.Migrations;

namespace Nadlan.Migrations
{
    public partial class changeItemstolist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lines_Items_ItemId",
                schema: "renovation",
                table: "Lines");

            migrationBuilder.DropIndex(
                name: "IX_Lines_ItemId",
                schema: "renovation",
                table: "Lines");

            migrationBuilder.DropColumn(
                name: "ItemId",
                schema: "renovation",
                table: "Lines");

            migrationBuilder.AddColumn<int>(
                name: "LineId",
                schema: "renovation",
                table: "Items",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_LineId",
                schema: "renovation",
                table: "Items",
                column: "LineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Lines_LineId",
                schema: "renovation",
                table: "Items",
                column: "LineId",
                principalSchema: "renovation",
                principalTable: "Lines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Lines_LineId",
                schema: "renovation",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_LineId",
                schema: "renovation",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "LineId",
                schema: "renovation",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                schema: "renovation",
                table: "Lines",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lines_ItemId",
                schema: "renovation",
                table: "Lines",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lines_Items_ItemId",
                schema: "renovation",
                table: "Lines",
                column: "ItemId",
                principalSchema: "renovation",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
