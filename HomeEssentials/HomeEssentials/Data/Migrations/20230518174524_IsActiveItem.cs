using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeEssentials.Data.Migrations
{
    public partial class IsActiveItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "19118075",
                table: "Items",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "19118075",
                table: "Items");
        }
    }
}
