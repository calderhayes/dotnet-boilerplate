using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Data.PostgreSQL.Migrations
{
    public partial class auditticketupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogEvents");

            migrationBuilder.AddColumn<string>(
                name: "RequestId",
                table: "AuditTickets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "AuditTickets");

            migrationBuilder.CreateTable(
                name: "LogEvents",
                columns: table => new
                {
                    LogEventId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AuditTicketId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEvents", x => x.LogEventId);
                    table.ForeignKey(
                        name: "FK_LogEvents_AuditTickets_AuditTicketId",
                        column: x => x.AuditTicketId,
                        principalTable: "AuditTickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogEvents_AuditTicketId",
                table: "LogEvents",
                column: "AuditTicketId");
        }
    }
}
