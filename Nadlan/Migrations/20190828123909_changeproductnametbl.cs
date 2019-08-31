using Microsoft.EntityFrameworkCore.Migrations;

namespace Nadlan.Migrations
{
    public partial class changeproductnametbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Product_ProductId",
                schema: "renovation",
                table: "Item");

            migrationBuilder.DropForeignKey(
                name: "FK_Line_Apartments_ApartmentId",
                schema: "renovation",
                table: "Line");

            migrationBuilder.DropForeignKey(
                name: "FK_Line_Item_ItemId",
                schema: "renovation",
                table: "Line");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                schema: "renovation",
                table: "Product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Line",
                schema: "renovation",
                table: "Line");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Item",
                schema: "renovation",
                table: "Item");

            migrationBuilder.RenameTable(
                name: "Product",
                schema: "renovation",
                newName: "Products",
                newSchema: "renovation");

            migrationBuilder.RenameTable(
                name: "Line",
                schema: "renovation",
                newName: "Lines",
                newSchema: "renovation");

            migrationBuilder.RenameTable(
                name: "Item",
                schema: "renovation",
                newName: "Items",
                newSchema: "renovation");

            migrationBuilder.RenameIndex(
                name: "IX_Line_ItemId",
                schema: "renovation",
                table: "Lines",
                newName: "IX_Lines_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Line_ApartmentId",
                schema: "renovation",
                table: "Lines",
                newName: "IX_Lines_ApartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Item_ProductId",
                schema: "renovation",
                table: "Items",
                newName: "IX_Items_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                schema: "renovation",
                table: "Products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lines",
                schema: "renovation",
                table: "Lines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Items",
                schema: "renovation",
                table: "Items",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Products_ProductId",
                schema: "renovation",
                table: "Items",
                column: "ProductId",
                principalSchema: "renovation",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lines_Apartments_ApartmentId",
                schema: "renovation",
                table: "Lines",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Products_ProductId",
                schema: "renovation",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Lines_Apartments_ApartmentId",
                schema: "renovation",
                table: "Lines");

            migrationBuilder.DropForeignKey(
                name: "FK_Lines_Items_ItemId",
                schema: "renovation",
                table: "Lines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                schema: "renovation",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lines",
                schema: "renovation",
                table: "Lines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Items",
                schema: "renovation",
                table: "Items");

            migrationBuilder.RenameTable(
                name: "Products",
                schema: "renovation",
                newName: "Product",
                newSchema: "renovation");

            migrationBuilder.RenameTable(
                name: "Lines",
                schema: "renovation",
                newName: "Line",
                newSchema: "renovation");

            migrationBuilder.RenameTable(
                name: "Items",
                schema: "renovation",
                newName: "Item",
                newSchema: "renovation");

            migrationBuilder.RenameIndex(
                name: "IX_Lines_ItemId",
                schema: "renovation",
                table: "Line",
                newName: "IX_Line_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Lines_ApartmentId",
                schema: "renovation",
                table: "Line",
                newName: "IX_Line_ApartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_ProductId",
                schema: "renovation",
                table: "Item",
                newName: "IX_Item_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                schema: "renovation",
                table: "Product",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Line",
                schema: "renovation",
                table: "Line",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Item",
                schema: "renovation",
                table: "Item",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Product_ProductId",
                schema: "renovation",
                table: "Item",
                column: "ProductId",
                principalSchema: "renovation",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Line_Apartments_ApartmentId",
                schema: "renovation",
                table: "Line",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Line_Item_ItemId",
                schema: "renovation",
                table: "Line",
                column: "ItemId",
                principalSchema: "renovation",
                principalTable: "Item",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
