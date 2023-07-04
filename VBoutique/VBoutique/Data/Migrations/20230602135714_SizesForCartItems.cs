using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VBoutique.Data.Migrations
{
    public partial class SizesForCartItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                schema: "19118155",
                table: "ShoppingCartItems");

            migrationBuilder.AddColumn<int>(
                name: "Size35Count",
                schema: "19118155",
                table: "ShoppingCartItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Size36Count",
                schema: "19118155",
                table: "ShoppingCartItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Size37Count",
                schema: "19118155",
                table: "ShoppingCartItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Size38Count",
                schema: "19118155",
                table: "ShoppingCartItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Size39Count",
                schema: "19118155",
                table: "ShoppingCartItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Size40Count",
                schema: "19118155",
                table: "ShoppingCartItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Size41Count",
                schema: "19118155",
                table: "ShoppingCartItems",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size35Count",
                schema: "19118155",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "Size36Count",
                schema: "19118155",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "Size37Count",
                schema: "19118155",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "Size38Count",
                schema: "19118155",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "Size39Count",
                schema: "19118155",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "Size40Count",
                schema: "19118155",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "Size41Count",
                schema: "19118155",
                table: "ShoppingCartItems");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                schema: "19118155",
                table: "ShoppingCartItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
