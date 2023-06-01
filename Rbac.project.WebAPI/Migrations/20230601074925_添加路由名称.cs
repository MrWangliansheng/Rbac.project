using Microsoft.EntityFrameworkCore.Migrations;

namespace Rbac.project.WebAPI.Migrations
{
    public partial class 添加路由名称 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RouteName",
                table: "Power",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RouteName",
                table: "Power");
        }
    }
}
