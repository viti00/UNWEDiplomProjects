using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessProgram.Migrations
{
    public partial class AddReservationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeRanges_Reservations_ReservationId",
                schema: "19118074",
                table: "TimeRanges");

            migrationBuilder.RenameColumn(
                name: "ReservationId",
                schema: "19118074",
                table: "TimeRanges",
                newName: "DateOfReservationId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeRanges_ReservationId",
                schema: "19118074",
                table: "TimeRanges",
                newName: "IX_TimeRanges_DateOfReservationId");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "19118074",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DateId",
                schema: "19118074",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                schema: "19118074",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "19118074",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RangeId",
                schema: "19118074",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "19118074",
                table: "Reservations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DateOfReservations",
                schema: "19118074",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateOfReservations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserId",
                schema: "19118074",
                table: "Reservations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_UserId",
                schema: "19118074",
                table: "Reservations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeRanges_DateOfReservations_DateOfReservationId",
                schema: "19118074",
                table: "TimeRanges",
                column: "DateOfReservationId",
                principalSchema: "19118074",
                principalTable: "DateOfReservations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_UserId",
                schema: "19118074",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeRanges_DateOfReservations_DateOfReservationId",
                schema: "19118074",
                table: "TimeRanges");

            migrationBuilder.DropTable(
                name: "DateOfReservations",
                schema: "19118074");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_UserId",
                schema: "19118074",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Address",
                schema: "19118074",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "DateId",
                schema: "19118074",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "FullName",
                schema: "19118074",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "19118074",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "RangeId",
                schema: "19118074",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "19118074",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "DateOfReservationId",
                schema: "19118074",
                table: "TimeRanges",
                newName: "ReservationId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeRanges_DateOfReservationId",
                schema: "19118074",
                table: "TimeRanges",
                newName: "IX_TimeRanges_ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeRanges_Reservations_ReservationId",
                schema: "19118074",
                table: "TimeRanges",
                column: "ReservationId",
                principalSchema: "19118074",
                principalTable: "Reservations",
                principalColumn: "Id");
        }
    }
}
