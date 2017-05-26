using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.PostgreSQL.Migrations
{
  public partial class responselog2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResquestLogs",
                columns: table => new
                {
                    AuditTicketId = table.Column<long>(nullable: false),
                    RequestBody = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResquestLogs", x => x.AuditTicketId);
                    table.ForeignKey(
                        name: "FK_ResquestLogs_AuditTickets_AuditTicketId",
                        column: x => x.AuditTicketId,
                        principalTable: "AuditTickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResponseLogs",
                columns: table => new
                {
                    AuditTicketId = table.Column<long>(nullable: false),
                    ResponseBody = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponseLogs", x => x.AuditTicketId);
                    table.ForeignKey(
                        name: "FK_ResponseLogs_AuditTickets_AuditTicketId",
                        column: x => x.AuditTicketId,
                        principalTable: "AuditTickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResquestLogs");

            migrationBuilder.DropTable(
                name: "ResponseLogs");
        }
    }
}
