using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.PostgreSQL.Migrations
{
    public partial class principalupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Domain",
                table: "PrincipalClosureMaps",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ExternalId",
                table: "Principals",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Principals",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Principals_ExternalId",
                table: "Principals",
                column: "ExternalId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Principals_ExternalId",
                table: "Principals");

            migrationBuilder.DropColumn(
                name: "Domain",
                table: "PrincipalClosureMaps");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Principals");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Principals");
        }
    }
}
