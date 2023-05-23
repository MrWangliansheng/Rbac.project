using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rbac.project.WebAPI.Migrations
{
    public partial class CreateRbacWatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Power",
                columns: table => new
                {
                    PowerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PowerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PowerRoute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PowerTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MesgCreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MsgCreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MegDeleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MsgDeleteTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MsgUpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MegUpdateTipme = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Power", x => x.PowerId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleCreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MesgCreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MsgCreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MegDeleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MsgDeleteTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MsgUpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MegUpdateTipme = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "RolePower",
                columns: table => new
                {
                    RolePowerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    PowerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePower", x => x.RolePowerId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserPwaword = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserCreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MesgCreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MsgCreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MegDeleteUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MsgDeleteTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MsgUpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MegUpdateTipme = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserRoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.UserRoleId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Power");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "RolePower");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "UserRole");
        }
    }
}
