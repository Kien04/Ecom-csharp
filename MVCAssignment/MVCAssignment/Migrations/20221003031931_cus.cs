using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCAssignment.Migrations
{
    public partial class cus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "Customer");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "CustomersTable");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomersTable",
                table: "CustomersTable",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomersTable",
                table: "CustomersTable");

            migrationBuilder.RenameTable(
                name: "CustomersTable",
                newName: "Customer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "Customer",
                column: "Id");
        }
    }
}
