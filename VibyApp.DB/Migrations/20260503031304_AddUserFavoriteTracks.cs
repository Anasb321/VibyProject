using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VibyApp.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddUserFavoriteTracks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserFavoriteTracks",
                columns: table => new
                {
                    FavoriteTracksId = table.Column<int>(type: "INTEGER", nullable: false),
                    FavoritedByUsersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavoriteTracks", x => new { x.FavoriteTracksId, x.FavoritedByUsersId });
                    table.ForeignKey(
                        name: "FK_UserFavoriteTracks_Tracks_FavoriteTracksId",
                        column: x => x.FavoriteTracksId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFavoriteTracks_Users_FavoritedByUsersId",
                        column: x => x.FavoritedByUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "MotDePasse",
                value: "$2a$11$MuU2pHv04GndUqx6PaIW2erIG9WLczpAG.OaztYrzHKP2xpJ35Vfe");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteTracks_FavoritedByUsersId",
                table: "UserFavoriteTracks",
                column: "FavoritedByUsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFavoriteTracks");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "MotDePasse",
                value: "$2a$11$Q0xfr88HMRoE84ZTLkbPluIPqRQ2Zrv4DBjMRBweB21fUJkHi9JUm");
        }
    }
}
