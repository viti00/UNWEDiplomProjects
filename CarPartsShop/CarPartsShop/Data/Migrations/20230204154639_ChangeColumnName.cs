using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarPartsShop.Data.Migrations
{
    public partial class ChangeColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastModified",
                schema: "19118067",
                table: "PickedUpParts",
                newName: "LastModified_19118067");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                schema: "19118067",
                table: "PartTypes",
                newName: "LastModified_19118067");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                schema: "19118067",
                table: "Parts",
                newName: "LastModified_19118067");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                schema: "19118067",
                table: "PartImages",
                newName: "LastModified_19118067");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                schema: "19118067",
                table: "PartConditions",
                newName: "LastModified_19118067");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                schema: "19118067",
                table: "OrderStatuses",
                newName: "LastModified_19118067");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                schema: "19118067",
                table: "Orders",
                newName: "LastModified_19118067");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                schema: "19118067",
                table: "CartStatuses",
                newName: "LastModified_19118067");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                schema: "19118067",
                table: "Carts",
                newName: "LastModified_19118067");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastModified_19118067",
                schema: "19118067",
                table: "PickedUpParts",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "LastModified_19118067",
                schema: "19118067",
                table: "PartTypes",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "LastModified_19118067",
                schema: "19118067",
                table: "Parts",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "LastModified_19118067",
                schema: "19118067",
                table: "PartImages",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "LastModified_19118067",
                schema: "19118067",
                table: "PartConditions",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "LastModified_19118067",
                schema: "19118067",
                table: "OrderStatuses",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "LastModified_19118067",
                schema: "19118067",
                table: "Orders",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "LastModified_19118067",
                schema: "19118067",
                table: "CartStatuses",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "LastModified_19118067",
                schema: "19118067",
                table: "Carts",
                newName: "LastModified");
        }
    }
}
