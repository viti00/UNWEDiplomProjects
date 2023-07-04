using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeEssentials.Data.Migrations
{
    public partial class ItemsReviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ItemId",
                schema: "19118075",
                table: "Reviews",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ItemId",
                schema: "19118075",
                table: "Reviews",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Items_ItemId",
                schema: "19118075",
                table: "Reviews",
                column: "ItemId",
                principalSchema: "19118075",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Items_ItemId",
                schema: "19118075",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ItemId",
                schema: "19118075",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ItemId",
                schema: "19118075",
                table: "Reviews");
        }
    }
}
