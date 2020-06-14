using Microsoft.EntityFrameworkCore.Migrations;

namespace Nadlan.Migrations
{
    public partial class tablenamefix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lines_projects_RenovationProjectId",
                schema: "renovation",
                table: "Lines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lines",
                schema: "renovation",
                table: "Lines");

            migrationBuilder.RenameTable(
                name: "Lines",
                schema: "renovation",
                newName: "lines",
                newSchema: "renovation");

            migrationBuilder.RenameIndex(
                name: "IX_Lines_RenovationProjectId",
                schema: "renovation",
                table: "lines",
                newName: "IX_lines_RenovationProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_lines",
                schema: "renovation",
                table: "lines",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_lines_projects_RenovationProjectId",
                schema: "renovation",
                table: "lines",
                column: "RenovationProjectId",
                principalSchema: "renovation",
                principalTable: "projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lines_projects_RenovationProjectId",
                schema: "renovation",
                table: "lines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_lines",
                schema: "renovation",
                table: "lines");

            migrationBuilder.RenameTable(
                name: "lines",
                schema: "renovation",
                newName: "Lines",
                newSchema: "renovation");

            migrationBuilder.RenameIndex(
                name: "IX_lines_RenovationProjectId",
                schema: "renovation",
                table: "Lines",
                newName: "IX_Lines_RenovationProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lines",
                schema: "renovation",
                table: "Lines",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lines_projects_RenovationProjectId",
                schema: "renovation",
                table: "Lines",
                column: "RenovationProjectId",
                principalSchema: "renovation",
                principalTable: "projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
