using Microsoft.EntityFrameworkCore.Migrations;

namespace Nadlan.Migrations
{
    public partial class capitaltables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_apartments_ApartmentId",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Issues_IssueId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Issues",
                table: "Issues");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "messages");

            migrationBuilder.RenameTable(
                name: "Issues",
                newName: "issues");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_IssueId",
                table: "messages",
                newName: "IX_messages_IssueId");

            migrationBuilder.RenameIndex(
                name: "IX_Issues_ApartmentId",
                table: "issues",
                newName: "IX_issues_ApartmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_messages",
                table: "messages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_issues",
                table: "issues",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_issues_apartments_ApartmentId",
                table: "issues",
                column: "ApartmentId",
                principalTable: "apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_messages_issues_IssueId",
                table: "messages",
                column: "IssueId",
                principalTable: "issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_issues_apartments_ApartmentId",
                table: "issues");

            migrationBuilder.DropForeignKey(
                name: "FK_messages_issues_IssueId",
                table: "messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_messages",
                table: "messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_issues",
                table: "issues");

            migrationBuilder.RenameTable(
                name: "messages",
                newName: "Messages");

            migrationBuilder.RenameTable(
                name: "issues",
                newName: "Issues");

            migrationBuilder.RenameIndex(
                name: "IX_messages_IssueId",
                table: "Messages",
                newName: "IX_Messages_IssueId");

            migrationBuilder.RenameIndex(
                name: "IX_issues_ApartmentId",
                table: "Issues",
                newName: "IX_Issues_ApartmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Issues",
                table: "Issues",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_apartments_ApartmentId",
                table: "Issues",
                column: "ApartmentId",
                principalTable: "apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Issues_IssueId",
                table: "Messages",
                column: "IssueId",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
