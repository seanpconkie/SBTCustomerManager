using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SBTCustomerManager.Data.Migrations
{
    public partial class FixCompanyEditMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDetails_Profile_ProfileId",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "UserRole",
                table: "UserDetails");

            migrationBuilder.AlterColumn<long>(
                name: "ProfileId",
                table: "UserDetails",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetails_Profile_ProfileId",
                table: "UserDetails",
                column: "ProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDetails_Profile_ProfileId",
                table: "UserDetails");

            migrationBuilder.AlterColumn<long>(
                name: "ProfileId",
                table: "UserDetails",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "UserRole",
                table: "UserDetails",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetails_Profile_ProfileId",
                table: "UserDetails",
                column: "ProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
