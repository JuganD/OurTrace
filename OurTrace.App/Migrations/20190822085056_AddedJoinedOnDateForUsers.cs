using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OurTrace.App.Migrations
{
    public partial class AddedJoinedOnDateForUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "JoinedOn",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JoinedOn",
                table: "AspNetUsers");
        }
    }
}
