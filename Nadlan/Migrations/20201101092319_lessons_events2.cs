using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nadlan.Migrations
{
    public partial class lessons_events2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "lessons",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "events",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "lessons",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "events",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "GETDATE()");
        }
    }
}
