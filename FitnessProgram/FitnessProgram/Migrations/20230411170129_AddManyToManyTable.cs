using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessProgram.Migrations
{
    public partial class AddManyToManyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeRanges_DateOfReservations_DateOfReservationId",
                schema: "19118074",
                table: "TimeRanges");

            migrationBuilder.DropIndex(
                name: "IX_TimeRanges_DateOfReservationId",
                schema: "19118074",
                table: "TimeRanges");

            migrationBuilder.DropColumn(
                name: "DateOfReservationId",
                schema: "19118074",
                table: "TimeRanges");

            migrationBuilder.CreateTable(
                name: "ReservationDateTimeRange",
                schema: "19118074",
                columns: table => new
                {
                    DateOfReservationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TimeRangeId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationDateTimeRange", x => new { x.DateOfReservationId, x.TimeRangeId });
                    table.ForeignKey(
                        name: "FK_ReservationDateTimeRange_DateOfReservations_DateOfReservationId",
                        column: x => x.DateOfReservationId,
                        principalSchema: "19118074",
                        principalTable: "DateOfReservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationDateTimeRange_TimeRanges_TimeRangeId",
                        column: x => x.TimeRangeId,
                        principalSchema: "19118074",
                        principalTable: "TimeRanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationDateTimeRange_TimeRangeId",
                schema: "19118074",
                table: "ReservationDateTimeRange",
                column: "TimeRangeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationDateTimeRange",
                schema: "19118074");

            migrationBuilder.AddColumn<string>(
                name: "DateOfReservationId",
                schema: "19118074",
                table: "TimeRanges",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TimeRanges_DateOfReservationId",
                schema: "19118074",
                table: "TimeRanges",
                column: "DateOfReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeRanges_DateOfReservations_DateOfReservationId",
                schema: "19118074",
                table: "TimeRanges",
                column: "DateOfReservationId",
                principalSchema: "19118074",
                principalTable: "DateOfReservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
