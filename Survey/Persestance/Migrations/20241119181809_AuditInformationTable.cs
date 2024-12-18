using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Survey.Persestance.Migrations
{
    /// <inheritdoc />
    public partial class AuditInformationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByID",
                table: "polls",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "polls",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByID",
                table: "polls",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "polls",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_polls_CreatedByID",
                table: "polls",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_polls_UpdatedByID",
                table: "polls",
                column: "UpdatedByID");

            migrationBuilder.AddForeignKey(
                name: "FK_polls_AspNetUsers_CreatedByID",
                table: "polls",
                column: "CreatedByID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_polls_AspNetUsers_UpdatedByID",
                table: "polls",
                column: "UpdatedByID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_polls_AspNetUsers_CreatedByID",
                table: "polls");

            migrationBuilder.DropForeignKey(
                name: "FK_polls_AspNetUsers_UpdatedByID",
                table: "polls");

            migrationBuilder.DropIndex(
                name: "IX_polls_CreatedByID",
                table: "polls");

            migrationBuilder.DropIndex(
                name: "IX_polls_UpdatedByID",
                table: "polls");

            migrationBuilder.DropColumn(
                name: "CreatedByID",
                table: "polls");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "polls");

            migrationBuilder.DropColumn(
                name: "UpdatedByID",
                table: "polls");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "polls");
        }
    }
}
