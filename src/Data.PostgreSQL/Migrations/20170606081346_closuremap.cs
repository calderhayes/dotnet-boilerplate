using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.PostgreSQL.Migrations
{
    public partial class closuremap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrincipalClosureMaps",
                columns: table => new
                {
                    AncestorId = table.Column<long>(nullable: false),
                    DescendantId = table.Column<long>(nullable: false),
                    CreatedTicketId = table.Column<long>(nullable: false),
                    PathLength = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrincipalClosureMaps", x => new { x.AncestorId, x.DescendantId });
                    table.ForeignKey(
                        name: "FK_PrincipalClosureMaps_Principals_AncestorId",
                        column: x => x.AncestorId,
                        principalTable: "Principals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrincipalClosureMaps_AuditTickets_CreatedTicketId",
                        column: x => x.CreatedTicketId,
                        principalTable: "AuditTickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrincipalClosureMaps_Principals_DescendantId",
                        column: x => x.DescendantId,
                        principalTable: "Principals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrincipalClosureMaps_CreatedTicketId",
                table: "PrincipalClosureMaps",
                column: "CreatedTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_PrincipalClosureMaps_DescendantId",
                table: "PrincipalClosureMaps",
                column: "DescendantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrincipalClosureMaps");
        }
    }
}
