using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rule.DAL.Migrations
{
    /// <inheritdoc />
    public partial class deleteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "PicturesPosts");

            migrationBuilder.AddColumn<byte[]>(
                name: "Pictures",
                table: "Users",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PicturesPosts",
                table: "Posts",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "Pictures",
                table: "Foundations",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pictures",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PicturesPosts",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Pictures",
                table: "Foundations");

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoundationsId = table.Column<byte>(type: "tinyint", nullable: true),
                    Picture = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    UsersId = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pictures_Foundations_FoundationsId",
                        column: x => x.FoundationsId,
                        principalTable: "Foundations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pictures_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PicturesPosts",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pictures = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PostsId = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PicturesPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PicturesPosts_Posts_PostsId",
                        column: x => x.PostsId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_FoundationsId",
                table: "Pictures",
                column: "FoundationsId");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_UsersId",
                table: "Pictures",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_PicturesPosts_PostsId",
                table: "PicturesPosts",
                column: "PostsId");
        }
    }
}
