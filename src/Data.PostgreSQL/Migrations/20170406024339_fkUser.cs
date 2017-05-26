using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.PostgreSQL.Migrations
{
  public partial class fkUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserAuthenticationSource_UserId",
                table: "UserAuthenticationSource",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAuthenticationSource_UserAccounts_UserId",
                table: "UserAuthenticationSource",
                column: "UserId",
                principalTable: "UserAccounts",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAuthenticationSource_UserAccounts_UserId",
                table: "UserAuthenticationSource");

            migrationBuilder.DropIndex(
                name: "IX_UserAuthenticationSource_UserId",
                table: "UserAuthenticationSource");
        }
    }
}
