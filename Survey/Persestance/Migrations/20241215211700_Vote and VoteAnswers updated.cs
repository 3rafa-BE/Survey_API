using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Survey.Persestance.Migrations
{
    /// <inheritdoc />
    public partial class VoteandVoteAnswersupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vote_AspNetUsers_UserId",
                table: "Vote");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_polls_pollid",
                table: "Vote");

            migrationBuilder.DropForeignKey(
                name: "FK_VoteAnswers_Vote_VoteId",
                table: "VoteAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_VoteAnswers_answers_AnswerId",
                table: "VoteAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_VoteAnswers_questions_QuestionId",
                table: "VoteAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VoteAnswers",
                table: "VoteAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vote",
                table: "Vote");

            migrationBuilder.RenameTable(
                name: "VoteAnswers",
                newName: "voteAnswers");

            migrationBuilder.RenameTable(
                name: "Vote",
                newName: "votes");

            migrationBuilder.RenameIndex(
                name: "IX_VoteAnswers_VoteId_QuestionId",
                table: "voteAnswers",
                newName: "IX_voteAnswers_VoteId_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_VoteAnswers_QuestionId",
                table: "voteAnswers",
                newName: "IX_voteAnswers_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_VoteAnswers_AnswerId",
                table: "voteAnswers",
                newName: "IX_voteAnswers_AnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_Vote_UserId_pollid",
                table: "votes",
                newName: "IX_votes_UserId_pollid");

            migrationBuilder.RenameIndex(
                name: "IX_Vote_pollid",
                table: "votes",
                newName: "IX_votes_pollid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_voteAnswers",
                table: "voteAnswers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_votes",
                table: "votes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_voteAnswers_answers_AnswerId",
                table: "voteAnswers",
                column: "AnswerId",
                principalTable: "answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_voteAnswers_questions_QuestionId",
                table: "voteAnswers",
                column: "QuestionId",
                principalTable: "questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_voteAnswers_votes_VoteId",
                table: "voteAnswers",
                column: "VoteId",
                principalTable: "votes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_votes_AspNetUsers_UserId",
                table: "votes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_votes_polls_pollid",
                table: "votes",
                column: "pollid",
                principalTable: "polls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_voteAnswers_answers_AnswerId",
                table: "voteAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_voteAnswers_questions_QuestionId",
                table: "voteAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_voteAnswers_votes_VoteId",
                table: "voteAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_votes_AspNetUsers_UserId",
                table: "votes");

            migrationBuilder.DropForeignKey(
                name: "FK_votes_polls_pollid",
                table: "votes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_voteAnswers",
                table: "voteAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_votes",
                table: "votes");

            migrationBuilder.RenameTable(
                name: "voteAnswers",
                newName: "VoteAnswers");

            migrationBuilder.RenameTable(
                name: "votes",
                newName: "Vote");

            migrationBuilder.RenameIndex(
                name: "IX_voteAnswers_VoteId_QuestionId",
                table: "VoteAnswers",
                newName: "IX_VoteAnswers_VoteId_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_voteAnswers_QuestionId",
                table: "VoteAnswers",
                newName: "IX_VoteAnswers_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_voteAnswers_AnswerId",
                table: "VoteAnswers",
                newName: "IX_VoteAnswers_AnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_votes_UserId_pollid",
                table: "Vote",
                newName: "IX_Vote_UserId_pollid");

            migrationBuilder.RenameIndex(
                name: "IX_votes_pollid",
                table: "Vote",
                newName: "IX_Vote_pollid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VoteAnswers",
                table: "VoteAnswers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vote",
                table: "Vote",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_AspNetUsers_UserId",
                table: "Vote",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_polls_pollid",
                table: "Vote",
                column: "pollid",
                principalTable: "polls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VoteAnswers_Vote_VoteId",
                table: "VoteAnswers",
                column: "VoteId",
                principalTable: "Vote",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VoteAnswers_answers_AnswerId",
                table: "VoteAnswers",
                column: "AnswerId",
                principalTable: "answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VoteAnswers_questions_QuestionId",
                table: "VoteAnswers",
                column: "QuestionId",
                principalTable: "questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
