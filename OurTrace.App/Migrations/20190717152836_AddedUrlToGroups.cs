using Microsoft.EntityFrameworkCore.Migrations;

namespace OurTrace.App.Migrations
{
    public partial class AddedUrlToGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Shares",
                table: "Groups");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Groups",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Groups");

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Groups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Shares",
                table: "Groups",
                nullable: false,
                defaultValue: 0);
        }
    }
}
