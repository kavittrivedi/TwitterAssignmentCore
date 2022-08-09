using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitterAssignmentApi.Migrations
{
    public partial class AddNewTableAndRelationShip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tweets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentTweetId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(144)", maxLength: 144, nullable: false),
                    TweetDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tweets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tweets_Tweets_ParentTweetId",
                        column: x => x.ParentTweetId,
                        principalTable: "Tweets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tweets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TweetReactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TweetId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ReactionType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TweetReactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TweetReactions_Tweets_TweetId",
                        column: x => x.TweetId,
                        principalTable: "Tweets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TweetReactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TweetReactions_TweetId",
                table: "TweetReactions",
                column: "TweetId");

            migrationBuilder.CreateIndex(
                name: "IX_TweetReactions_UserId",
                table: "TweetReactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tweets_ParentTweetId",
                table: "Tweets",
                column: "ParentTweetId");

            migrationBuilder.CreateIndex(
                name: "IX_Tweets_UserId",
                table: "Tweets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TweetReactions");

            migrationBuilder.DropTable(
                name: "Tweets");
        }
    }
}
