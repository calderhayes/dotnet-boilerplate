using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.PostgreSQL.Migrations
{
  public partial class userauthsourcefk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAuthenticationSource_UserAccounts_UserId",
                table: "UserAuthenticationSource");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAuthenticationSource",
                table: "UserAuthenticationSource");

            migrationBuilder.RenameTable(
                name: "UserAuthenticationSource",
                newName: "UserAuthenticationSources");

            migrationBuilder.RenameIndex(
                name: "IX_UserAuthenticationSource_UserId",
                table: "UserAuthenticationSources",
                newName: "IX_UserAuthenticationSources_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAuthenticationSources",
                table: "UserAuthenticationSources",
                columns: new[] { "AuthenticationSource", "Subject", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserAuthenticationSources_UserAccounts_UserId",
                table: "UserAuthenticationSources",
                column: "UserId",
                principalTable: "UserAccounts",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAuthenticationSources_UserAccounts_UserId",
                table: "UserAuthenticationSources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAuthenticationSources",
                table: "UserAuthenticationSources");

            migrationBuilder.RenameTable(
                name: "UserAuthenticationSources",
                newName: "UserAuthenticationSource");

            migrationBuilder.RenameIndex(
                name: "IX_UserAuthenticationSources_UserId",
                table: "UserAuthenticationSource",
                newName: "IX_UserAuthenticationSource_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAuthenticationSource",
                table: "UserAuthenticationSource",
                columns: new[] { "AuthenticationSource", "Subject", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserAuthenticationSource_UserAccounts_UserId",
                table: "UserAuthenticationSource",
                column: "UserId",
                principalTable: "UserAccounts",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
