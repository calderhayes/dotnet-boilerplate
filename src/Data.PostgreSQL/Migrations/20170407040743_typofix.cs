using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.PostgreSQL.Migrations
{
  public partial class typofix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResquestLogs_AuditTickets_AuditTicketId",
                table: "ResquestLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResquestLogs",
                table: "ResquestLogs");

            migrationBuilder.RenameTable(
                name: "ResquestLogs",
                newName: "RequestLogs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestLogs",
                table: "RequestLogs",
                column: "AuditTicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestLogs_AuditTickets_AuditTicketId",
                table: "RequestLogs",
                column: "AuditTicketId",
                principalTable: "AuditTickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestLogs_AuditTickets_AuditTicketId",
                table: "RequestLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestLogs",
                table: "RequestLogs");

            migrationBuilder.RenameTable(
                name: "RequestLogs",
                newName: "ResquestLogs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResquestLogs",
                table: "ResquestLogs",
                column: "AuditTicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResquestLogs_AuditTickets_AuditTicketId",
                table: "ResquestLogs",
                column: "AuditTicketId",
                principalTable: "AuditTickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
