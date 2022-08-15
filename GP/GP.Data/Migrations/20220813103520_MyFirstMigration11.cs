using Microsoft.EntityFrameworkCore.Migrations;

namespace GP.Data.Migrations
{
    public partial class MyFirstMigration11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Setup",
                table: "Businesses");

            migrationBuilder.AddColumn<string>(
                name: "Setup",
                table: "BusinessOwners",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Setup",
                table: "BusinessOwners");

            migrationBuilder.AddColumn<string>(
                name: "Setup",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
