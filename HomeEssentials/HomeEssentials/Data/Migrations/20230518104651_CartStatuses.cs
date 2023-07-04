using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeEssentials.Data.Migrations
{
    public partial class CartStatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CartStatusId",
                schema: "19118075",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CartStatuses",
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
                    table.PrimaryKey("PK_CartStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CartStatusId",
                schema: "19118075",
                table: "Carts",
                column: "CartStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_CartStatuses_CartStatusId",
                schema: "19118075",
                table: "Carts",
                column: "CartStatusId",
                principalSchema: "19118075",
                principalTable: "CartStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_CartStatuses_CartStatusId",
                schema: "19118075",
                table: "Carts");

            migrationBuilder.DropTable(
                name: "CartStatuses",
                schema: "19118075");

            migrationBuilder.DropIndex(
                name: "IX_Carts_CartStatusId",
                schema: "19118075",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "CartStatusId",
                schema: "19118075",
                table: "Carts");
        }
    }
}
