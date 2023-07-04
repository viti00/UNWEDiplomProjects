using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessProgram.Migrations
{
    public partial class AddLastModifiedField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers",
                schema: "19118074");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "UserLikedPosts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "TimeRanges",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "Reservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "ReservationDateTimeRange",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "ProfilePhotos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "PostPhoto",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "Partners",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "PartnerPhotos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "DateOfReservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "BestResults",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "BestResultPhotos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified_19118074",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "UserLikedPosts");

            migrationBuilder.DropColumn(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "TimeRanges");

            migrationBuilder.DropColumn(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "ReservationDateTimeRange");

            migrationBuilder.DropColumn(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "ProfilePhotos");

            migrationBuilder.DropColumn(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "PostPhoto");

            migrationBuilder.DropColumn(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "PartnerPhotos");

            migrationBuilder.DropColumn(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "DateOfReservations");

            migrationBuilder.DropColumn(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "BestResults");

            migrationBuilder.DropColumn(
                name: "LastModified_19118074",
                schema: "19118074",
                table: "BestResultPhotos");

            migrationBuilder.DropColumn(
                name: "LastModified_19118074",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "19118074",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    DesiredResults = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Sex = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                schema: "19118074",
                table: "Customers",
                column: "UserId");
        }
    }
}
