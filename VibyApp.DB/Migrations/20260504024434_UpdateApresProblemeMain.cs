using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VibyApp.DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateApresProblemeMain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "MotDePasse",
                value: "$2a$11$il3QAwJKmK8MRBA.UKzq4eX/keDhq4RqZFruFno/a/uOpEF9EUFfK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "MotDePasse",
                value: "$2a$11$azVxUWCLSpeP2pdjmbJe2ub3FXQiF9M8/lnjga85Vcy.UUsXaippa");
        }
    }
}
