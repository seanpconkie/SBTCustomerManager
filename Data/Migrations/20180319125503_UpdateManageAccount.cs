using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SBTCustomerManager.Data.Migrations
{
    public partial class UpdateManageAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "UserDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_CompanyId",
                table: "UserDetails",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetails_CompanyDetails_CompanyId",
                table: "UserDetails",
                column: "CompanyId",
                principalTable: "CompanyDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDetails_CompanyDetails_CompanyId",
                table: "UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserDetails_CompanyId",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "UserDetails");
        }

    }
}
