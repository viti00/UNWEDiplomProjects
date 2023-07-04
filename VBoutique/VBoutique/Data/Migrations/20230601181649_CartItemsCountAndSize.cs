using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VBoutique.Data.Migrations
{
    public partial class CartItemsCountAndSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductCount",
                schema: "19118155",
                table: "ShoppingCartItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                schema: "19118155",
                table: "ShoppingCartItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCount",
                schema: "19118155",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "Size",
                schema: "19118155",
                table: "ShoppingCartItems");
        }
    }
}
