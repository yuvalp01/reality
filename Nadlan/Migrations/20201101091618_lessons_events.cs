using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nadlan.Migrations
{
    public partial class lessons_events : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    ApartmentId = table.Column<int>(nullable: false),
                    Severity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "lessons",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    ApartmentId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    ReferenceId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lessons", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.DropTable(
                name: "lessons");
        }
    }
}
