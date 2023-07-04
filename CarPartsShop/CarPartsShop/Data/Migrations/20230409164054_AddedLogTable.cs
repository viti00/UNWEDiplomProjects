using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarPartsShop.Data.Migrations
{
    public partial class AddedLogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CVV",
                schema: "19118067",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CardHolder",
                schema: "19118067",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CardNumber",
                schema: "19118067",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ExpirationMonth",
                schema: "19118067",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ExpirationYear",
                schema: "19118067",
                table: "Orders");

            migrationBuilder.CreateTable(
                name: "log_19118067",
                schema: "19118067",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperationCreateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_19118067", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "log_19118067",
                schema: "19118067");

            migrationBuilder.AddColumn<string>(
                name: "CVV",
                schema: "19118067",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardHolder",
                schema: "19118067",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                schema: "19118067",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpirationMonth",
                schema: "19118067",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpirationYear",
                schema: "19118067",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
