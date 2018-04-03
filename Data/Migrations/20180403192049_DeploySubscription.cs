using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SBTCustomerManager.Data.Migrations
{
    public partial class DeploySubscription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleTypes_Types_TypeId",
                table: "RoleTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Types",
                table: "Types");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleTypes",
                table: "RoleTypes");

            migrationBuilder.RenameTable(
                name: "Types",
                newName: "RoleType");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "RoleTypes",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleType",
                table: "RoleType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleTypes",
                table: "RoleTypes",
                column: "RoleId");

            migrationBuilder.CreateTable(
                name: "SubscriptionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BillingFrequency = table.Column<int>(nullable: false),
                    CompanyId = table.Column<long>(nullable: false, defaultValue: 0),
                    Cost = table.Column<decimal>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    SubscriptionTypeId = table.Column<int>(nullable: true, defaultValue: 0),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Users = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_SubscriptionTypes_SubscriptionTypeId",
                        column: x => x.SubscriptionTypeId,
                        principalTable: "SubscriptionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SubscriptionTypeId",
                table: "Subscriptions",
                column: "SubscriptionTypeId");


            migrationBuilder.AddForeignKey(
                name: "FK_RoleTypes_RoleType_TypeId",
                table: "RoleTypes",
                column: "Id",
                principalTable: "RoleType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "SubscriptionTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleTypes",
                table: "RoleTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleType",
                table: "RoleType");

            migrationBuilder.RenameTable(
                name: "RoleType",
                newName: "Types");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RoleTypes",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "RoleTypes",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleTypes",
                table: "RoleTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Types",
                table: "Types",
                column: "Id");
        }
    }
}
