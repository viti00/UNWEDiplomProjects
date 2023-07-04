﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessProgram.Migrations
{
    public partial class ReservationStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReserevationStatus",
                schema: "19118074",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReserevationStatus",
                schema: "19118074",
                table: "Reservations");
        }
    }
}
