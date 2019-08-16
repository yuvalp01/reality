using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nadlan.Migrations
{
    public partial class MoreFieldsForApartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CurrentRent",
                table: "Apartments",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Door",
                table: "Apartments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Floor",
                table: "Apartments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "Apartments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "Apartments",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentRent",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "Door",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "Floor",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Apartments");
        }
    }
}
