using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SteliTour.Data.Migrations
{
    public partial class MakeStatusNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservations",
                schema: "19118105",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_DestinationId",
                schema: "19118105",
                table: "Reservations");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "19118105",
                table: "Destinations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservations",
                schema: "19118105",
                table: "Reservations",
                columns: new[] { "DestinationId", "UserId" });

            migrationBuilder.CreateTable(
                name: "log_19118105",
                schema: "19118105",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Operation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Table = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_19118105", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "log_19118105",
                schema: "19118105");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservations",
                schema: "19118105",
                table: "Reservations");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "19118105",
                table: "Destinations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservations",
                schema: "19118105",
                table: "Reservations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_DestinationId",
                schema: "19118105",
                table: "Reservations",
                column: "DestinationId");
        }
    }
}
