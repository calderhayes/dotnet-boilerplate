using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.PostgreSQL.Migrations
{
  public partial class ticketstuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreatedTicketId",
                table: "UserAuthenticationSources",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedTicketId",
                table: "UserAuthenticationSources",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthenticationSources_CreatedTicketId",
                table: "UserAuthenticationSources",
                column: "CreatedTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthenticationSources_ModifiedTicketId",
                table: "UserAuthenticationSources",
                column: "ModifiedTicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAuthenticationSources_AuditTickets_CreatedTicketId",
                table: "UserAuthenticationSources",
                column: "CreatedTicketId",
                principalTable: "AuditTickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAuthenticationSources_AuditTickets_ModifiedTicketId",
                table: "UserAuthenticationSources",
                column: "ModifiedTicketId",
                principalTable: "AuditTickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAuthenticationSources_AuditTickets_CreatedTicketId",
                table: "UserAuthenticationSources");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAuthenticationSources_AuditTickets_ModifiedTicketId",
                table: "UserAuthenticationSources");

            migrationBuilder.DropIndex(
                name: "IX_UserAuthenticationSources_CreatedTicketId",
                table: "UserAuthenticationSources");

            migrationBuilder.DropIndex(
                name: "IX_UserAuthenticationSources_ModifiedTicketId",
                table: "UserAuthenticationSources");

            migrationBuilder.DropColumn(
                name: "CreatedTicketId",
                table: "UserAuthenticationSources");

            migrationBuilder.DropColumn(
                name: "ModifiedTicketId",
                table: "UserAuthenticationSources");
        }
    }
}
