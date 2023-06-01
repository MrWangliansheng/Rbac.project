using Microsoft.EntityFrameworkCore.Migrations;

namespace Rbac.project.WebAPI.Migrations
{
    public partial class 添加菜单接口URL字段和图标字段 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PowerAPIUrl",
                table: "Power",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PowerIcon",
                table: "Power",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PowerAPIUrl",
                table: "Power");

            migrationBuilder.DropColumn(
                name: "PowerIcon",
                table: "Power");
        }
    }
}
