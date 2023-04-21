using Microsoft.EntityFrameworkCore.Migrations;

namespace BuildingShop_DataAccess.Migrations
{
    public partial class addAppTypeToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Product_AppId",
                table: "Product",
                column: "AppId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_AppType_AppId",
                table: "Product",
                column: "AppId",
                principalTable: "AppType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_AppType_AppId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_AppId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "AppId",
                table: "Product");
        }
    }
}
