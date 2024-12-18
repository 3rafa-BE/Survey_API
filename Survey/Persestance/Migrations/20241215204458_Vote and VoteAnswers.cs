using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Survey.Persestance.Migrations
{
    /// <inheritdoc />
    public partial class VoteandVoteAnswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    pollid = table.Column<int>(type: "int", nullable: false),
                    SubmittedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vote_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vote_polls_pollid",
                        column: x => x.pollid,
                        principalTable: "polls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VoteAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerId = table.Column<int>(type: "int", nullable: false),
                    VoteId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoteAnswers_Vote_VoteId",
                        column: x => x.VoteId,
                        principalTable: "Vote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VoteAnswers_answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VoteAnswers_questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vote_pollid",
                table: "Vote",
                column: "pollid");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_UserId_pollid",
                table: "Vote",
                columns: new[] { "UserId", "pollid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VoteAnswers_AnswerId",
                table: "VoteAnswers",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_VoteAnswers_QuestionId",
                table: "VoteAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_VoteAnswers_VoteId_QuestionId",
                table: "VoteAnswers",
                columns: new[] { "VoteId", "QuestionId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VoteAnswers");

            migrationBuilder.DropTable(
                name: "Vote");
        }
    }
}
