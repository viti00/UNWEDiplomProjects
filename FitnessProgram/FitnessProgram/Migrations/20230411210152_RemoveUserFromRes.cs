using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessProgram.Migrations
{
    public partial class RemoveUserFromRes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationDateTimeRange_AspNetUsers_UserId",
                schema: "19118074",
                table: "ReservationDateTimeRange");

            migrationBuilder.DropIndex(
                name: "IX_ReservationDateTimeRange_UserId",
                schema: "19118074",
                table: "ReservationDateTimeRange");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "19118074",
                table: "ReservationDateTimeRange");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "19118074",
                table: "ReservationDateTimeRange",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationDateTimeRange_UserId",
                schema: "19118074",
                table: "ReservationDateTimeRange",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationDateTimeRange_AspNetUsers_UserId",
                schema: "19118074",
                table: "ReservationDateTimeRange",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
