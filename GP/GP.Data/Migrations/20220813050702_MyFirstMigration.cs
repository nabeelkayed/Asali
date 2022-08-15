using Microsoft.EntityFrameworkCore.Migrations;

namespace GP.Data.Migrations
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessOwners_Businesses_BusinessOwnerId",
                table: "BusinessOwners");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "Reviews",
                newName: "ReviewText");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessOwners_BusinessId",
                table: "BusinessOwners",
                column: "BusinessId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessOwners_Businesses_BusinessId",
                table: "BusinessOwners",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "BusinessId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessOwners_Businesses_BusinessId",
                table: "BusinessOwners");

            migrationBuilder.DropIndex(
                name: "IX_BusinessOwners_BusinessId",
                table: "BusinessOwners");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ReviewText",
                table: "Reviews",
                newName: "Body");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessOwners_Businesses_BusinessOwnerId",
                table: "BusinessOwners",
                column: "BusinessOwnerId",
                principalTable: "Businesses",
                principalColumn: "BusinessId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
