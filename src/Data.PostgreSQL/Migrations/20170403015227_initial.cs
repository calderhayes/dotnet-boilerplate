using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Data.PostgreSQL.Migrations
{
  public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditTickets",
                columns: table => new
                {
                    TicketId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    EndTime = table.Column<DateTimeOffset>(nullable: true),
                    IpAddress = table.Column<string>(nullable: true),
                    SecurityTokenId = table.Column<string>(nullable: true),
                    SecurityTokenIssuer = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTimeOffset>(nullable: false),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTickets", x => x.TicketId);
                });

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

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedTicketId = table.Column<long>(nullable: false),
                    ModifiedTicketId = table.Column<long>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserAccounts_AuditTickets_CreatedTicketId",
                        column: x => x.CreatedTicketId,
                        principalTable: "AuditTickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAccounts_AuditTickets_ModifiedTicketId",
                        column: x => x.ModifiedTicketId,
                        principalTable: "AuditTickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogEvents_AuditTicketId",
                table: "LogEvents",
                column: "AuditTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_CreatedTicketId",
                table: "UserAccounts",
                column: "CreatedTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_ModifiedTicketId",
                table: "UserAccounts",
                column: "ModifiedTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_UserName",
                table: "UserAccounts",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogEvents");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "AuditTickets");
        }
    }
}
