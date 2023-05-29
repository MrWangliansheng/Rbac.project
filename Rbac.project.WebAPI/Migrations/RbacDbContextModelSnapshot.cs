﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Rbac.project.Repoistorys;

namespace Rbac.project.WebAPI.Migrations
{
    [DbContext(typeof(RbacDbContext))]
    partial class RbacDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Rbac.project.Domain.LogData", b =>
                {
                    b.Property<int>("LogDataId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("LogMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Operator")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LogDataId");

                    b.ToTable("LogData");
                });

            modelBuilder.Entity("Rbac.project.Domain.Power", b =>
                {
                    b.Property<int>("PowerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("MegDeleteUser")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("MegUpdateTipme")
                        .HasColumnType("datetime2");

                    b.Property<string>("MesgCreateUser")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("MsgCreateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("MsgDeleteTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("MsgUpdateUser")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PowerDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PowerIsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("PowerName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("PowerParentId")
                        .HasColumnType("int");

                    b.Property<string>("PowerParentIdAll")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PowerRoute")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("PowerTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("PowerType")
                        .HasColumnType("int");

                    b.HasKey("PowerId");

                    b.ToTable("Power");
                });

            modelBuilder.Entity("Rbac.project.Domain.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("MegDeleteUser")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("MegUpdateTipme")
                        .HasColumnType("datetime2");

                    b.Property<string>("MesgCreateUser")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("MsgCreateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("MsgDeleteTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("MsgUpdateUser")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("RoleCreateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("RoleIsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("RoleName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("RoleParentId")
                        .HasColumnType("int");

                    b.Property<string>("RoleParentIdAll")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Rbac.project.Domain.RolePower", b =>
                {
                    b.Property<int>("RolePowerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PowerID")
                        .HasColumnType("int");

                    b.Property<int>("RoleID")
                        .HasColumnType("int");

                    b.HasKey("RolePowerId");

                    b.ToTable("RolePower");
                });

            modelBuilder.Entity("Rbac.project.Domain.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LastLoginIP")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("LastLoginTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("MegDeleteUser")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("MegUpdateTipme")
                        .HasColumnType("datetime2");

                    b.Property<string>("MesgCreateUser")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("MsgCreateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("MsgDeleteTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("MsgUpdateUser")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PullName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("UserCreateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserEmail")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("UserImg")
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<bool>("UserIsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserPassword")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("UserState")
                        .HasColumnType("bit");

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Rbac.project.Domain.UserRole", b =>
                {
                    b.Property<int>("UserRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("RoleID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("UserRoleId");

                    b.ToTable("UserRole");
                });
#pragma warning restore 612, 618
        }
    }
}
