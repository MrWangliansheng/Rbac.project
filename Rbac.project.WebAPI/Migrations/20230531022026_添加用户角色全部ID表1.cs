using Microsoft.EntityFrameworkCore.Migrations;

namespace Rbac.project.WebAPI.Migrations
{
    public partial class 添加用户角色全部ID表1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRoleIdAll",
                columns: table => new
                {
                    URAID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleIdAll = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleIdAll", x => x.URAID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRoleIdAll");
        }
    }
}
