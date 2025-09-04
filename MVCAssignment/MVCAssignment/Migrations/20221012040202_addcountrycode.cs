using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCAssignment.Migrations
{
    public partial class addcountrycode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "CustomersTabel",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "CustomersTabel");
        }
    }
}
