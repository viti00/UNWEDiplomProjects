using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessProgram.Migrations
{
    public partial class AddStatusProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "19118074",
                table: "TimeRanges");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "19118074",
                table: "ReservationDateTimeRange",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "19118074",
                table: "ReservationDateTimeRange");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "19118074",
                table: "TimeRanges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
