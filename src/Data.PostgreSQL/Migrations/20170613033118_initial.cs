using System;
using System.Collections.Generic;
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
                    DisplayUrl = table.Column<string>(nullable: true),
                    EndTime = table.Column<DateTimeOffset>(nullable: true),
                    IpAddress = table.Column<string>(nullable: true),
                    RequestId = table.Column<string>(nullable: true),
                    RequestMethod = table.Column<string>(nullable: true),
                    SecurityTokenId = table.Column<string>(nullable: true),
                    SecurityTokenIssuer = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTimeOffset>(nullable: false),
                    UserAgent = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTickets", x => x.TicketId);
                });

            migrationBuilder.CreateTable(
                name: "SecurityProfileToggles",
                columns: table => new
                {
                    ToggleType = table.Column<int>(nullable: false),
                    Category = table.Column<int>(nullable: false),
                    IsDynamic = table.Column<bool>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityProfileToggles", x => x.ToggleType);
                });

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

            migrationBuilder.CreateTable(
                name: "Nodes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedTicketId = table.Column<long>(nullable: false),
                    ExternalId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Label = table.Column<string>(nullable: false),
                    ModifiedTicketId = table.Column<long>(nullable: false),
                    NodeType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nodes_AuditTickets_CreatedTicketId",
                        column: x => x.CreatedTicketId,
                        principalTable: "AuditTickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Nodes_AuditTickets_ModifiedTicketId",
                        column: x => x.ModifiedTicketId,
                        principalTable: "AuditTickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestLogs",
                columns: table => new
                {
                    AuditTicketId = table.Column<long>(nullable: false),
                    RequestBody = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLogs", x => x.AuditTicketId);
                    table.ForeignKey(
                        name: "FK_RequestLogs_AuditTickets_AuditTicketId",
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

            migrationBuilder.CreateTable(
                name: "SecurityProfiles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedTicketId = table.Column<long>(nullable: false),
                    ExternalId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsSystem = table.Column<bool>(nullable: false),
                    Label = table.Column<string>(nullable: false),
                    ModifiedTicketId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecurityProfiles_AuditTickets_CreatedTicketId",
                        column: x => x.CreatedTicketId,
                        principalTable: "AuditTickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SecurityProfiles_AuditTickets_ModifiedTicketId",
                        column: x => x.ModifiedTicketId,
                        principalTable: "AuditTickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NodeClosureMaps",
                columns: table => new
                {
                    AncestorId = table.Column<long>(nullable: false),
                    DescendantId = table.Column<long>(nullable: false),
                    CreatedTicketId = table.Column<long>(nullable: false),
                    Domain = table.Column<int>(nullable: false),
                    PathLength = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodeClosureMaps", x => new { x.AncestorId, x.DescendantId });
                    table.ForeignKey(
                        name: "FK_NodeClosureMaps_Nodes_AncestorId",
                        column: x => x.AncestorId,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NodeClosureMaps_AuditTickets_CreatedTicketId",
                        column: x => x.CreatedTicketId,
                        principalTable: "AuditTickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NodeClosureMaps_Nodes_DescendantId",
                        column: x => x.DescendantId,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecurityProfileToggleMaps",
                columns: table => new
                {
                    SecurityProfileId = table.Column<long>(nullable: false),
                    ToggleType = table.Column<int>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityProfileToggleMaps", x => new { x.SecurityProfileId, x.ToggleType });
                    table.ForeignKey(
                        name: "FK_SecurityProfileToggleMaps_SecurityProfiles_SecurityProfileId",
                        column: x => x.SecurityProfileId,
                        principalTable: "SecurityProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SecurityProfileToggleMaps_SecurityProfileToggles_ToggleType",
                        column: x => x.ToggleType,
                        principalTable: "SecurityProfileToggles",
                        principalColumn: "ToggleType",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    CreatedTicketId = table.Column<long>(nullable: false),
                    Culture = table.Column<string>(nullable: false),
                    ModifiedTicketId = table.Column<long>(nullable: false),
                    SecurityProfileId = table.Column<long>(nullable: false),
                    UserName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAccounts_AuditTickets_CreatedTicketId",
                        column: x => x.CreatedTicketId,
                        principalTable: "AuditTickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAccounts_Nodes_Id",
                        column: x => x.Id,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAccounts_AuditTickets_ModifiedTicketId",
                        column: x => x.ModifiedTicketId,
                        principalTable: "AuditTickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAccounts_SecurityProfiles_SecurityProfileId",
                        column: x => x.SecurityProfileId,
                        principalTable: "SecurityProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAuthenticationSources",
                columns: table => new
                {
                    AuthenticationSource = table.Column<string>(nullable: false),
                    Subject = table.Column<string>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    CreatedTicketId = table.Column<long>(nullable: false),
                    ModifiedTicketId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthenticationSources", x => new { x.AuthenticationSource, x.Subject, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserAuthenticationSources_AuditTickets_CreatedTicketId",
                        column: x => x.CreatedTicketId,
                        principalTable: "AuditTickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAuthenticationSources_AuditTickets_ModifiedTicketId",
                        column: x => x.ModifiedTicketId,
                        principalTable: "AuditTickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAuthenticationSources_UserAccounts_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HubLogs_AuditTicketId",
                table: "HubLogs",
                column: "AuditTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_CreatedTicketId",
                table: "Nodes",
                column: "CreatedTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_ExternalId",
                table: "Nodes",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_ModifiedTicketId",
                table: "Nodes",
                column: "ModifiedTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_NodeClosureMaps_CreatedTicketId",
                table: "NodeClosureMaps",
                column: "CreatedTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_NodeClosureMaps_DescendantId",
                table: "NodeClosureMaps",
                column: "DescendantId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityProfiles_CreatedTicketId",
                table: "SecurityProfiles",
                column: "CreatedTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityProfiles_ExternalId",
                table: "SecurityProfiles",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SecurityProfiles_ModifiedTicketId",
                table: "SecurityProfiles",
                column: "ModifiedTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityProfileToggleMaps_ToggleType",
                table: "SecurityProfileToggleMaps",
                column: "ToggleType");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_CreatedTicketId",
                table: "UserAccounts",
                column: "CreatedTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_ModifiedTicketId",
                table: "UserAccounts",
                column: "ModifiedTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_SecurityProfileId",
                table: "UserAccounts",
                column: "SecurityProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_UserName",
                table: "UserAccounts",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthenticationSources_CreatedTicketId",
                table: "UserAuthenticationSources",
                column: "CreatedTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthenticationSources_ModifiedTicketId",
                table: "UserAuthenticationSources",
                column: "ModifiedTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthenticationSources_UserId",
                table: "UserAuthenticationSources",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HubLogs");

            migrationBuilder.DropTable(
                name: "NodeClosureMaps");

            migrationBuilder.DropTable(
                name: "RequestLogs");

            migrationBuilder.DropTable(
                name: "ResponseLogs");

            migrationBuilder.DropTable(
                name: "SecurityProfileToggleMaps");

            migrationBuilder.DropTable(
                name: "UserAuthenticationSources");

            migrationBuilder.DropTable(
                name: "SecurityProfileToggles");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "Nodes");

            migrationBuilder.DropTable(
                name: "SecurityProfiles");

            migrationBuilder.DropTable(
                name: "AuditTickets");
        }
    }
}
