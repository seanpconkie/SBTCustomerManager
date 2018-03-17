using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SBTCustomerManager.Data.Migrations
{
    public partial class UpdateUserDetailsUserContactId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDetails_UserContacts_UserContactId1",
                table: "UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserDetails_UserContactId1",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "UserContactId1",
                table: "UserDetails");

            migrationBuilder.AlterColumn<int>(
                name: "UserContactId",
                table: "UserDetails",
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_UserContactId",
                table: "UserDetails",
                column: "UserContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetails_UserContacts_UserContactId",
                table: "UserDetails",
                column: "UserContactId",
                principalTable: "UserContacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropColumn(
                name: "ForeName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "AspNetUsers");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDetails_UserContacts_UserContactId",
                table: "UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserDetails_UserContactId",
                table: "UserDetails");

            migrationBuilder.AlterColumn<byte>(
                name: "UserContactId",
                table: "UserDetails",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "UserContactId1",
                table: "UserDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_UserContactId1",
                table: "UserDetails",
                column: "UserContactId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetails_UserContacts_UserContactId1",
                table: "UserDetails",
                column: "UserContactId1",
                principalTable: "UserContacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
