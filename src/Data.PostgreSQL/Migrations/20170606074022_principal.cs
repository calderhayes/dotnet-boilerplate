using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Data.PostgreSQL.Migrations
{
    public partial class principal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAuthenticationSources_UserAccounts_UserId",
                table: "UserAuthenticationSources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAccounts",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserAccounts");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "UserAccounts",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAccounts",
                table: "UserAccounts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Principals",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedTicketId = table.Column<long>(nullable: false),
                    Label = table.Column<string>(nullable: false),
                    ModifiedTicketId = table.Column<long>(nullable: false),
                    PrincipalType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Principals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Principals_AuditTickets_CreatedTicketId",
                        column: x => x.CreatedTicketId,
                        principalTable: "AuditTickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Principals_AuditTickets_ModifiedTicketId",
                        column: x => x.ModifiedTicketId,
                        principalTable: "AuditTickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Principals_CreatedTicketId",
                table: "Principals",
                column: "CreatedTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Principals_ModifiedTicketId",
                table: "Principals",
                column: "ModifiedTicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_Principals_Id",
                table: "UserAccounts",
                column: "Id",
                principalTable: "Principals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAuthenticationSources_UserAccounts_UserId",
                table: "UserAuthenticationSources",
                column: "UserId",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Principals_Id",
                table: "UserAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAuthenticationSources_UserAccounts_UserId",
                table: "UserAuthenticationSources");

            migrationBuilder.DropTable(
                name: "Principals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAccounts",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserAccounts");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "UserAccounts",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAccounts",
                table: "UserAccounts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAuthenticationSources_UserAccounts_UserId",
                table: "UserAuthenticationSources",
                column: "UserId",
                principalTable: "UserAccounts",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
