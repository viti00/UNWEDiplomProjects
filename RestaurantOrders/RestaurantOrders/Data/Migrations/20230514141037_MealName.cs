using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantOrders.Data.Migrations
{
    public partial class MealName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MealName",
                schema: "19118066",
                table: "Meals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MealName",
                schema: "19118066",
                table: "Meals");
        }
    }
}
