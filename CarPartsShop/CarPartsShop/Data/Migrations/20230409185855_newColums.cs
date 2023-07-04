using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarPartsShop.Data.Migrations
{
    public partial class newColums : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                schema: "19118067",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                schema: "19118067",
                table: "Orders");
        }
    }
}
