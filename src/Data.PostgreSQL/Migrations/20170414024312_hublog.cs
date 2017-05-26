using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Data.PostgreSQL.Migrations
{
    public partial class hublog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HubLogs",
                columns: table => new
                {
                    HubLogId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Arguments = table.Column<string>(nullable: true),
                    AuditTicketId = table.Column<long>(nullable: true),
                    HubName = table.Column<string>(nullable: true),
                    IsIncoming = table.Column<bool>(nullable: false),
                    MethodName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubLogs", x => x.HubLogId);
                    table.ForeignKey(
                        name: "FK_HubLogs_AuditTickets_AuditTicketId",
                        column: x => x.AuditTicketId,
                        principalTable: "AuditTickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HubLogs_AuditTicketId",
                table: "HubLogs",
                column: "AuditTicketId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HubLogs");
        }
    }
}
