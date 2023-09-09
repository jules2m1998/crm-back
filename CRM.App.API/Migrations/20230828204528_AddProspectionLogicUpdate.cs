using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.App.API.Migrations
{
    /// <inheritdoc />
    public partial class AddProspectionLogicUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commits_StageResponses_QuetionId",
                table: "Commits");

            migrationBuilder.RenameColumn(
                name: "QuetionId",
                table: "Commits",
                newName: "ResponseId");

            migrationBuilder.RenameIndex(
                name: "IX_Commits_QuetionId",
                table: "Commits",
                newName: "IX_Commits_ResponseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commits_StageResponses_ResponseId",
                table: "Commits",
                column: "ResponseId",
                principalTable: "StageResponses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commits_StageResponses_ResponseId",
                table: "Commits");

            migrationBuilder.RenameColumn(
                name: "ResponseId",
                table: "Commits",
                newName: "QuetionId");

            migrationBuilder.RenameIndex(
                name: "IX_Commits_ResponseId",
                table: "Commits",
                newName: "IX_Commits_QuetionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commits_StageResponses_QuetionId",
                table: "Commits",
                column: "QuetionId",
                principalTable: "StageResponses",
                principalColumn: "Id");
        }
    }
}
