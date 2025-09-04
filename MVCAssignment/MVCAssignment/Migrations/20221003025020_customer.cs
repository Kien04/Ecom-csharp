using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCAssignment.Migrations
{
    public partial class customer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "ProductsTable");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsTable",
                table: "ProductsTable",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsTable",
                table: "ProductsTable");

            migrationBuilder.RenameTable(
                name: "ProductsTable",
                newName: "Product");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "Id");
        }
    }
}
