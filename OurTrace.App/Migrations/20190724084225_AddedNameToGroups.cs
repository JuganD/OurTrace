using Microsoft.EntityFrameworkCore.Migrations;

namespace OurTrace.App.Migrations
{
    public partial class AddedNameToGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Groups",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Groups");
        }
    }
}
