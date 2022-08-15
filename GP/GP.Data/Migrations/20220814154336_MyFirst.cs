using Microsoft.EntityFrameworkCore.Migrations;

namespace GP.Data.Migrations
{
    public partial class MyFirst : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessUsername",
                table: "Businesses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BusinessUsername",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
