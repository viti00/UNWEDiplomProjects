using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessProgram.Migrations
{
    public partial class LastAvailableForModification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastAvailableForModification",
                schema: "19118074",
                table: "Reservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastAvailableForModification",
                schema: "19118074",
                table: "Reservations");
        }
    }
}
