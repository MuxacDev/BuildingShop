using Microsoft.EntityFrameworkCore.Migrations;

namespace BuildingShop_DataAccess.Migrations
{
    public partial class addAppTypeToProduct_Rename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_AppType_AppId",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "AppId",
                table: "Product",
                newName: "AppTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_AppId",
                table: "Product",
                newName: "IX_Product_AppTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_AppType_AppTypeId",
                table: "Product",
                column: "AppTypeId",
                principalTable: "AppType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_AppType_AppTypeId",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "AppTypeId",
                table: "Product",
                newName: "AppId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_AppTypeId",
                table: "Product",
                newName: "IX_Product_AppId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_AppType_AppId",
                table: "Product",
                column: "AppId",
                principalTable: "AppType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
