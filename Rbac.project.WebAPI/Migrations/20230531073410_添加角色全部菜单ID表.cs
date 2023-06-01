using Microsoft.EntityFrameworkCore.Migrations;

namespace Rbac.project.WebAPI.Migrations
{
    public partial class 添加角色全部菜单ID表 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RolePowerIdAll",
                columns: table => new
                {
                    RolePowerIdAllId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    PowerIdAll = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePowerIdAll", x => x.RolePowerIdAllId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePowerIdAll");
        }
    }
}
