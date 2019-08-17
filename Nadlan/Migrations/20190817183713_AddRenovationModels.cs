using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nadlan.Migrations
{
    public partial class AddRenovationModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RenovationItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Cost = table.Column<decimal>(nullable: false),
                    link = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenovationItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RenovationLine",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    WorkCost = table.Column<decimal>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    RenovationItemId = table.Column<int>(nullable: false),
                    ApartmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenovationLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RenovationLine_Apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RenovationLine_RenovationItem_RenovationItemId",
                        column: x => x.RenovationItemId,
                        principalTable: "RenovationItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RenovationLine_ApartmentId",
                table: "RenovationLine",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_RenovationLine_RenovationItemId",
                table: "RenovationLine",
                column: "RenovationItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RenovationLine");

            migrationBuilder.DropTable(
                name: "RenovationItem");
        }
    }
}
