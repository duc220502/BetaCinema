using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetaCinema.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatediscussion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovieReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    IsSpoiler = table.Column<bool>(type: "bit", nullable: false),
                    IsVerifiedPurchase = table.Column<bool>(type: "bit", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    LoveCount = table.Column<int>(type: "int", nullable: false),
                    HahaCount = table.Column<int>(type: "int", nullable: false),
                    WowCount = table.Column<int>(type: "int", nullable: false),
                    SadCount = table.Column<int>(type: "int", nullable: false),
                    AngryCount = table.Column<int>(type: "int", nullable: false),
                    ReactionCount = table.Column<int>(type: "int", nullable: false),
                    CommentCount = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeleteAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MovieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieReviews", x => x.Id);
                    table.CheckConstraint("CK_MovieReviews_Rating", "[Rating] IS NULL OR [Rating] BETWEEN 1 AND 10");
                    table.ForeignKey(
                        name: "FK_MovieReviews_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovieReviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReviewComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    LoveCount = table.Column<int>(type: "int", nullable: false),
                    HahaCount = table.Column<int>(type: "int", nullable: false),
                    WowCount = table.Column<int>(type: "int", nullable: false),
                    SadCount = table.Column<int>(type: "int", nullable: false),
                    AngryCount = table.Column<int>(type: "int", nullable: false),
                    ReactionCount = table.Column<int>(type: "int", nullable: false),
                    ReplyCount = table.Column<int>(type: "int", nullable: false),
                    Depth = table.Column<int>(type: "int", nullable: false),
                    RootCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReplyToCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MentionUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewComments", x => x.Id);
                    table.CheckConstraint("CK_ReviewComments_Depth", "[Depth] IN (0,1,2)");
                    table.CheckConstraint("CK_ReviewComments_Depth_Relation", "([Depth] = 0 AND [ParentCommentId] IS NULL AND [RootCommentId] IS NULL AND [ReplyToCommentId] IS NULL) OR ([Depth] IN (1,2) AND [ParentCommentId] IS NOT NULL AND [RootCommentId] IS NOT NULL AND [ReplyToCommentId] IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_ReviewComments_MovieReviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "MovieReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReviewComments_ReviewComments_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "ReviewComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReviewComments_ReviewComments_ReplyToCommentId",
                        column: x => x.ReplyToCommentId,
                        principalTable: "ReviewComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReviewComments_ReviewComments_RootCommentId",
                        column: x => x.RootCommentId,
                        principalTable: "ReviewComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReviewComments_Users_MentionUserId",
                        column: x => x.MentionUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReviewComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReactionType = table.Column<int>(type: "int", nullable: false),
                    ReviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.Id);
                    table.CheckConstraint("CK_Reactions_Target_OnlyOne", "(([ReviewId] IS NOT NULL AND [CommentId] IS NULL) OR ([ReviewId] IS NULL AND [CommentId] IS NOT NULL))");
                    table.ForeignKey(
                        name: "FK_Reactions_MovieReviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "MovieReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reactions_ReviewComments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "ReviewComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieReviews_MovieId_Status_CreateAt",
                table: "MovieReviews",
                columns: new[] { "MovieId", "Status", "CreateAt" });

            migrationBuilder.CreateIndex(
                name: "IX_MovieReviews_UserId_MovieId",
                table: "MovieReviews",
                columns: new[] { "UserId", "MovieId" },
                unique: true,
                filter: "[DeleteAt] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MovieReviews_UserId_Status",
                table: "MovieReviews",
                columns: new[] { "UserId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_CommentId_ReactionType",
                table: "Reactions",
                columns: new[] { "CommentId", "ReactionType" });

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_ReviewId_ReactionType",
                table: "Reactions",
                columns: new[] { "ReviewId", "ReactionType" });

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_UserId_CommentId",
                table: "Reactions",
                columns: new[] { "UserId", "CommentId" },
                unique: true,
                filter: "[CommentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_UserId_ReviewId",
                table: "Reactions",
                columns: new[] { "UserId", "ReviewId" },
                unique: true,
                filter: "[ReviewId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewComments_MentionUserId",
                table: "ReviewComments",
                column: "MentionUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewComments_ParentCommentId_Status_CreatedAt",
                table: "ReviewComments",
                columns: new[] { "ParentCommentId", "Status", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewComments_ReplyToCommentId",
                table: "ReviewComments",
                column: "ReplyToCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewComments_ReviewId_Depth_Status_CreatedAt",
                table: "ReviewComments",
                columns: new[] { "ReviewId", "Depth", "Status", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewComments_RootCommentId_Status_CreatedAt",
                table: "ReviewComments",
                columns: new[] { "RootCommentId", "Status", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewComments_UserId_CreatedAt",
                table: "ReviewComments",
                columns: new[] { "UserId", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reactions");

            migrationBuilder.DropTable(
                name: "ReviewComments");

            migrationBuilder.DropTable(
                name: "MovieReviews");
        }
    }
}
