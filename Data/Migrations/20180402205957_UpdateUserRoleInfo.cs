using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SBTCustomerManager.Data.Migrations
{
    public partial class UpdateUserRoleInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleDescriptions",
                columns: table => new
                {
                    RoleId = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleDescriptions", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "RoleTypes",
                columns: table => new
                {
                Id = table.Column<int>(nullable: false, defaultValue: 0),
                RoleId = table.Column<string>(nullable: false,maxLength: 255)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleTypes", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.Id);
                });

            migrationBuilder.Sql(@"
set identity_insert Types on
insert into Types(id,Type) values(0,'Private')       
set identity_insert Types off
insert into Types (type) values('Public')     
");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleTypes_Types_TypeId",
                table: "RoleTypes",
                column: "Id",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleDescriptions");

            migrationBuilder.DropTable(
                name: "RoleTypes");

            migrationBuilder.DropTable(
                name: "Types");
        }
    }
}
