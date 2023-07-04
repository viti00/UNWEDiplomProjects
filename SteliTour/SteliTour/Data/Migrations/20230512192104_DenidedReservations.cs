using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SteliTour.Data.Migrations
{
    public partial class DenidedReservations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DenidedReservations",
                schema: "19118105",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EGN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdultCount = table.Column<int>(type: "int", nullable: false),
                    KidsCount = table.Column<int>(type: "int", nullable: false),
                    Under2YearsKidsCount = table.Column<int>(type: "int", nullable: false),
                    Payed = table.Column<double>(type: "float", nullable: true),
                    Remainign = table.Column<double>(type: "float", nullable: true),
                    DateOfReservation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfCancelation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified_19118105 = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DenidedReservations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DenidedReservations",
                schema: "19118105");
        }
    }
}
