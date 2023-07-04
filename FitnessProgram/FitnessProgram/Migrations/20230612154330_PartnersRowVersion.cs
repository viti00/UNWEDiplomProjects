using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessProgram.Migrations
{
    public partial class PartnersRowVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RowVersion",
                schema: "19118074",
                table: "Partners",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "19118074",
                table: "Partners");
        }
    }
}
