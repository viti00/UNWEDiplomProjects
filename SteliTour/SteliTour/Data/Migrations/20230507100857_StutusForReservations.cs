using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SteliTour.Data.Migrations
{
    public partial class StutusForReservations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "19118105",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "19118105",
                table: "Reservations");
        }
    }
}
