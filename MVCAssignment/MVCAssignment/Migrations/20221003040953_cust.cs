using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCAssignment.Migrations
{
    public partial class cust : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomersTable",
                table: "CustomersTable");

            migrationBuilder.RenameTable(
                name: "CustomersTable",
                newName: "CustomersTabel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomersTabel",
                table: "CustomersTabel",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomersTabel",
                table: "CustomersTabel");

            migrationBuilder.RenameTable(
                name: "CustomersTabel",
                newName: "CustomersTable");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomersTable",
                table: "CustomersTable",
                column: "Id");
        }
    }
}
