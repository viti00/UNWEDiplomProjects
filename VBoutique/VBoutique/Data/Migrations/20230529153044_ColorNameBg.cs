using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VBoutique.Data.Migrations
{
    public partial class ColorNameBg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ColorName",
                schema: "19118155",
                table: "Colors",
                newName: "ColorNameLat");

            migrationBuilder.AddColumn<string>(
                name: "ColorNameBg",
                schema: "19118155",
                table: "Colors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorNameBg",
                schema: "19118155",
                table: "Colors");

            migrationBuilder.RenameColumn(
                name: "ColorNameLat",
                schema: "19118155",
                table: "Colors",
                newName: "ColorName");
        }
    }
}
