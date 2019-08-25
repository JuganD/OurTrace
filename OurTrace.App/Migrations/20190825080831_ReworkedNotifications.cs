using Microsoft.EntityFrameworkCore.Migrations;

namespace OurTrace.App.Migrations
{
    public partial class ReworkedNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Seen",
                table: "Notifications",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Seen",
                table: "Notifications");
        }
    }
}
