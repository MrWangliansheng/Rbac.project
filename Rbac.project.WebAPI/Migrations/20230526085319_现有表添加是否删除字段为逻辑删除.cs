using Microsoft.EntityFrameworkCore.Migrations;

namespace Rbac.project.WebAPI.Migrations
{
    public partial class 现有表添加是否删除字段为逻辑删除 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UserIsDelete",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RoleIsDelete",
                table: "Role",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PowerIsDelete",
                table: "Power",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserIsDelete",
                table: "User");

            migrationBuilder.DropColumn(
                name: "RoleIsDelete",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "PowerIsDelete",
                table: "Power");
        }
    }
}
