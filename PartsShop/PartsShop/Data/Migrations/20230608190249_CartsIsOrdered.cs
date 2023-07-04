using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartsShop.Data.Migrations
{
    public partial class CartsIsOrdered : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOrdered",
                schema: "19118073",
                table: "Carts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOrdered",
                schema: "19118073",
                table: "Carts");
        }
    }
}
