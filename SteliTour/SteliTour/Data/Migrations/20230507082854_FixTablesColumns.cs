using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SteliTour.Data.Migrations
{
    public partial class FixTablesColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EarnestPerPerson",
                schema: "19118105",
                table: "Destinations");

            migrationBuilder.AddColumn<string>(
                name: "EGN",
                schema: "19118105",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "19118105",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "19118105",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EGN",
                schema: "19118105",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "19118105",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "19118105",
                table: "Reservations");

            migrationBuilder.AddColumn<double>(
                name: "EarnestPerPerson",
                schema: "19118105",
                table: "Destinations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
