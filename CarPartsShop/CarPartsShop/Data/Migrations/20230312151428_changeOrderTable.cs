using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarPartsShop.Data.Migrations
{
    public partial class changeOrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                schema: "19118067",
                table: "PickedUpParts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "19118067",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
                name: "City",
                schema: "19118067",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.AddColumn<string>(
                name: "PostCode",
                schema: "19118067",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PickedUpParts_OrderId",
                schema: "19118067",
                table: "PickedUpParts",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_PickedUpParts_Orders_OrderId",
                schema: "19118067",
                table: "PickedUpParts",
                column: "OrderId",
                principalSchema: "19118067",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PickedUpParts_Orders_OrderId",
                schema: "19118067",
                table: "PickedUpParts");

            migrationBuilder.DropIndex(
                name: "IX_PickedUpParts_OrderId",
                schema: "19118067",
                table: "PickedUpParts");

            migrationBuilder.DropColumn(
                name: "OrderId",
                schema: "19118067",
                table: "PickedUpParts");

            migrationBuilder.DropColumn(
                name: "Address",
                schema: "19118067",
                table: "Orders");

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
                name: "City",
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

            migrationBuilder.DropColumn(
                name: "PostCode",
                schema: "19118067",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
