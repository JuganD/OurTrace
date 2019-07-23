using Microsoft.EntityFrameworkCore.Migrations;

namespace OurTrace.App.Migrations
{
    public partial class AddedImageOnFileSystemProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsImageOnFileSystem",
                table: "Posts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsImageOnFileSystem",
                table: "Posts");
        }
    }
}
