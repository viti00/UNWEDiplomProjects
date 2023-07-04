using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantOrders.Data.Migrations
{
    public partial class CategoriesConnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                schema: "19118066",
                table: "Meals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Meals_CategoryId",
                schema: "19118066",
                table: "Meals",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_Categories_CategoryId",
                schema: "19118066",
                table: "Meals",
                column: "CategoryId",
                principalSchema: "19118066",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meals_Categories_CategoryId",
                schema: "19118066",
                table: "Meals");

            migrationBuilder.DropIndex(
                name: "IX_Meals_CategoryId",
                schema: "19118066",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "19118066",
                table: "Meals");
        }
    }
}
