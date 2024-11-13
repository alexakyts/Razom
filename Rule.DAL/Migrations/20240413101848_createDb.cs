using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rule.DAL.Migrations
{
    /// <inheritdoc />
    public partial class createDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Foundations",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "text", maxLength: 1500, nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foundations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatusPosts",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusPosts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypePosts",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypePosts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Picture = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    FoundationsId = table.Column<byte>(type: "tinyint", nullable: true),
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
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "text", maxLength: 1500, nullable: false),
                    FinishSum = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsersId = table.Column<byte>(type: "tinyint", nullable: false),
                    StatusPostId = table.Column<byte>(type: "tinyint", nullable: false),
                    TypePostId = table.Column<byte>(type: "tinyint", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FoundationsId = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Foundations_FoundationsId",
                        column: x => x.FoundationsId,
                        principalTable: "Foundations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_StatusPosts_StatusPostId",
                        column: x => x.StatusPostId,
                        principalTable: "StatusPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_TypePosts_TypePostId",
                        column: x => x.TypePostId,
                        principalTable: "TypePosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_Posts_FoundationsId",
                table: "Posts",
                column: "FoundationsId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_StatusPostId",
                table: "Posts",
                column: "StatusPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TypePostId",
                table: "Posts",
                column: "TypePostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UsersId",
                table: "Posts",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "PicturesPosts");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Foundations");

            migrationBuilder.DropTable(
                name: "StatusPosts");

            migrationBuilder.DropTable(
                name: "TypePosts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
