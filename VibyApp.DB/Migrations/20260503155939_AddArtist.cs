using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VibyApp.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddArtist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ArtistPicture = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserFavoriteArtists",
                columns: table => new
                {
                    FavoriteArtistsId = table.Column<int>(type: "INTEGER", nullable: false),
                    FollowedByUsersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavoriteArtists", x => new { x.FavoriteArtistsId, x.FollowedByUsersId });
                    table.ForeignKey(
                        name: "FK_UserFavoriteArtists_Artists_FavoriteArtistsId",
                        column: x => x.FavoriteArtistsId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFavoriteArtists_Users_FollowedByUsersId",
                        column: x => x.FollowedByUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "MotDePasse",
                value: "$2a$11$azVxUWCLSpeP2pdjmbJe2ub3FXQiF9M8/lnjga85Vcy.UUsXaippa");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteArtists_FollowedByUsersId",
                table: "UserFavoriteArtists",
                column: "FollowedByUsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFavoriteArtists");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "MotDePasse",
                value: "$2a$11$MuU2pHv04GndUqx6PaIW2erIG9WLczpAG.OaztYrzHKP2xpJ35Vfe");
        }
    }
}
