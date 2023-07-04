using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VBoutique.Data.Migrations
{
    public partial class ColorNameEn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ColorNameLat",
                schema: "19118155",
                table: "Colors",
                newName: "ColorNameEn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ColorNameEn",
                schema: "19118155",
                table: "Colors",
                newName: "ColorNameLat");
        }
    }
}
