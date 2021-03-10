using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Identity.Migrations
{
    public partial class AddCountryColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "AddressUser",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "AddressUser");
        }
    }
}
