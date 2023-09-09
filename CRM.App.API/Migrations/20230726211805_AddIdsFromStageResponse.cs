using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.App.API.Migrations
{
    /// <inheritdoc />
    public partial class AddIdsFromStageResponse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StageResponses_StageQuetions_QuetionId",
                table: "StageResponses");

            migrationBuilder.DropIndex(
                name: "IX_StageResponses_QuetionId",
                table: "StageResponses");

            migrationBuilder.DropColumn(
                name: "QuetionId",
                table: "StageResponses");

            migrationBuilder.CreateIndex(
                name: "IX_StageResponses_QuestionId",
                table: "StageResponses",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_StageResponses_StageQuetions_QuestionId",
                table: "StageResponses",
                column: "QuestionId",
                principalTable: "StageQuetions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StageResponses_StageQuetions_QuestionId",
                table: "StageResponses");

            migrationBuilder.DropIndex(
                name: "IX_StageResponses_QuestionId",
                table: "StageResponses");

            migrationBuilder.AddColumn<Guid>(
                name: "QuetionId",
                table: "StageResponses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_StageResponses_QuetionId",
                table: "StageResponses",
                column: "QuetionId");

            migrationBuilder.AddForeignKey(
                name: "FK_StageResponses_StageQuetions_QuetionId",
                table: "StageResponses",
                column: "QuetionId",
                principalTable: "StageQuetions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
