using Microsoft.EntityFrameworkCore.Migrations;

namespace Nadlan.Migrations
{
    public partial class newInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                schema: "secure",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserClaims",
                schema: "secure",
                table: "UserClaims");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "secure",
                newName: "users",
                newSchema: "secure");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                schema: "secure",
                newName: "userClaims",
                newSchema: "secure");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                schema: "secure",
                table: "users",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_userClaims",
                schema: "secure",
                table: "userClaims",
                column: "ClaimId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                schema: "secure",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userClaims",
                schema: "secure",
                table: "userClaims");

            migrationBuilder.RenameTable(
                name: "users",
                schema: "secure",
                newName: "Users",
                newSchema: "secure");

            migrationBuilder.RenameTable(
                name: "userClaims",
                schema: "secure",
                newName: "UserClaims",
                newSchema: "secure");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                schema: "secure",
                table: "Users",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserClaims",
                schema: "secure",
                table: "UserClaims",
                column: "ClaimId");
        }
    }
}
