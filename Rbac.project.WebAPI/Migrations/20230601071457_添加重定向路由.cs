using Microsoft.EntityFrameworkCore.Migrations;

namespace Rbac.project.WebAPI.Migrations
{
    public partial class 添加重定向路由 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PowerRedirect",
                table: "Power",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PowerRedirect",
                table: "Power");
        }
    }
}
