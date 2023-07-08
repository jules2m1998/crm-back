using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.App.API.Migrations
{
    /// <inheritdoc />
    public partial class AddProductProspectionSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FirstStageId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductStages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDone = table.Column<bool>(type: "bit", nullable: false),
                    IsFirst = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActivated = table.Column<bool>(type: "bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductStages_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StageQuetions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuetionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActivated = table.Column<bool>(type: "bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "StageResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuetionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NextStageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActivated = table.Column<bool>(type: "bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StageResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StageResponses_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StageResponses_ProductStages_NextStageId",
                        column: x => x.NextStageId,
                        principalTable: "ProductStages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StageResponses_StageQuetions_QuetionId",
                        column: x => x.QuetionId,
                        principalTable: "StageQuetions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_FirstStageId",
                table: "Products",
                column: "FirstStageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStages_CreatorId",
                table: "ProductStages",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_StageQuetions_CreatorId",
                table: "StageQuetions",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_StageQuetions_QuetionId",
                table: "StageQuetions",
                column: "QuetionId");

            migrationBuilder.CreateIndex(
                name: "IX_StageResponses_CreatorId",
                table: "StageResponses",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_StageResponses_NextStageId",
                table: "StageResponses",
                column: "NextStageId");

            migrationBuilder.CreateIndex(
                name: "IX_StageResponses_QuetionId",
                table: "StageResponses",
                column: "QuetionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductStages_FirstStageId",
                table: "Products",
                column: "FirstStageId",
                principalTable: "ProductStages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductStages_FirstStageId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "StageResponses");

            migrationBuilder.DropTable(
                name: "StageQuetions");

            migrationBuilder.DropTable(
                name: "ProductStages");

            migrationBuilder.DropIndex(
                name: "IX_Products_FirstStageId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FirstStageId",
                table: "Products");
        }
    }
}
