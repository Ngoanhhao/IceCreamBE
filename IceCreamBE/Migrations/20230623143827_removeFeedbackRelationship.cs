using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IceCreamBE.Migrations
{
    /// <inheritdoc />
    public partial class removeFeedbackRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Accounts_AccountID",
                table: "Feedback");

            migrationBuilder.DropIndex(
                name: "IX_Feedback_AccountID",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "AccountID",
                table: "Feedback");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Feedback",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Feedback");

            migrationBuilder.AddColumn<int>(
                name: "AccountID",
                table: "Feedback",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_AccountID",
                table: "Feedback",
                column: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Accounts_AccountID",
                table: "Feedback",
                column: "AccountID",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
