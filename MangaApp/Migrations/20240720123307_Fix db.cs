using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaApp.Migrations
{
    /// <inheritdoc />
    public partial class Fixdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentLikes",
                table: "CommentLikes");

            migrationBuilder.AddColumn<Guid>(
                name: "CommentsCommentId",
                table: "CommentLikes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentLikes",
                table: "CommentLikes",
                columns: new[] { "CommentLikeId", "CommentId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_CommentLikes_CommentsCommentId",
                table: "CommentLikes",
                column: "CommentsCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentLikes_Comments_CommentsCommentId",
                table: "CommentLikes",
                column: "CommentsCommentId",
                principalTable: "Comments",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentLikes_Comments_CommentsCommentId",
                table: "CommentLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentLikes",
                table: "CommentLikes");

            migrationBuilder.DropIndex(
                name: "IX_CommentLikes_CommentsCommentId",
                table: "CommentLikes");

            migrationBuilder.DropColumn(
                name: "CommentsCommentId",
                table: "CommentLikes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentLikes",
                table: "CommentLikes",
                column: "CommentLikeId");
        }
    }
}
