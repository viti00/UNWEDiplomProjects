﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodAdditivesShop.Data.Migrations
{
    public partial class OrdersStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "19118119",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "19118119",
                table: "Orders");
        }
    }
}
