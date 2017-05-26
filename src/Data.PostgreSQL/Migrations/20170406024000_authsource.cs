using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.PostgreSQL.Migrations
{
  public partial class authsource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAuthenticationSource",
                columns: table => new
                {
                    AuthenticationSource = table.Column<string>(nullable: false),
                    Subject = table.Column<string>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthenticationSource", x => new { x.AuthenticationSource, x.Subject, x.UserId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAuthenticationSource");
        }
    }
}
