using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessProgram.Migrations
{
    public partial class AllosNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeRanges_Reservations_ReservationId",
                schema: "19118074",
                table: "TimeRanges");

            migrationBuilder.AlterColumn<string>(
                name: "ReservationId",
                schema: "19118074",
                table: "TimeRanges",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeRanges_Reservations_ReservationId",
                schema: "19118074",
                table: "TimeRanges",
                column: "ReservationId",
                principalSchema: "19118074",
                principalTable: "Reservations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeRanges_Reservations_ReservationId",
                schema: "19118074",
                table: "TimeRanges");

            migrationBuilder.AlterColumn<string>(
                name: "ReservationId",
                schema: "19118074",
                table: "TimeRanges",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeRanges_Reservations_ReservationId",
                schema: "19118074",
                table: "TimeRanges",
                column: "ReservationId",
                principalSchema: "19118074",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
