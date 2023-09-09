using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.App.API.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringOfProductStageBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StageResponses_StageQuetions_QuestionId",
                table: "StageResponses");

            migrationBuilder.DropTable(
                name: "StageQuetions");

            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "ProductStages");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "StageResponses",
                newName: "StageId");

            migrationBuilder.RenameIndex(
                name: "IX_StageResponses_QuestionId",
                table: "StageResponses",
                newName: "IX_StageResponses_StageId");

            migrationBuilder.AddColumn<string>(
                name: "Question",
                table: "ProductStages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_StageResponses_ProductStages_StageId",
                table: "StageResponses",
                column: "StageId",
                principalTable: "ProductStages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StageResponses_ProductStages_StageId",
                table: "StageResponses");

            migrationBuilder.DropColumn(
                name: "Question",
                table: "ProductStages");

            migrationBuilder.RenameColumn(
                name: "StageId",
                table: "StageResponses",
                newName: "QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_StageResponses_StageId",
                table: "StageResponses",
                newName: "IX_StageResponses_QuestionId");

            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "ProductStages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "StageQuetions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    QuetionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActivated = table.Column<bool>(type: "bit", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StageQuetions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StageQuetions_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StageQuetions_ProductStages_QuetionId",
                        column: x => x.QuetionId,
                        principalTable: "ProductStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StageQuetions_CreatorId",
                table: "StageQuetions",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_StageQuetions_QuetionId",
                table: "StageQuetions",
                column: "QuetionId");

            migrationBuilder.AddForeignKey(
                name: "FK_StageResponses_StageQuetions_QuestionId",
                table: "StageResponses",
                column: "QuestionId",
                principalTable: "StageQuetions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
