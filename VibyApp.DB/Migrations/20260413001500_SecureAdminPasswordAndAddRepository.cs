using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VibyApp.DB.Migrations
{
    /// <inheritdoc />
    public partial class SecureAdminPasswordAndAddRepository : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "MotDePasse",
                value: "$2a$11$Q0xfr88HMRoE84ZTLkbPluIPqRQ2Zrv4DBjMRBweB21fUJkHi9JUm");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "MotDePasse",
                value: "1234");
        }
    }
}
