using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeEssentials.Data.Migrations
{
    public partial class OrderStatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                schema: "19118075",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderStatuses",
                schema: "19118075",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModified_19118075 = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StatusId",
                schema: "19118075",
                table: "Orders",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderStatuses_StatusId",
                schema: "19118075",
                table: "Orders",
                column: "StatusId",
                principalSchema: "19118075",
                principalTable: "OrderStatuses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderStatuses_StatusId",
                schema: "19118075",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "OrderStatuses",
                schema: "19118075");

            migrationBuilder.DropIndex(
                name: "IX_Orders_StatusId",
                schema: "19118075",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StatusId",
                schema: "19118075",
                table: "Orders");
        }
    }
}
