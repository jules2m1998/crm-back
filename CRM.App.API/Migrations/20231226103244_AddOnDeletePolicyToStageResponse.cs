using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.App.API.Migrations
{
    /// <inheritdoc />
    public partial class AddOnDeletePolicyToStageResponse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StageResponses_ProductStages_StageId",
                table: "StageResponses");

            migrationBuilder.AddForeignKey(
                name: "FK_StageResponses_ProductStages_StageId",
                table: "StageResponses",
                column: "StageId",
                principalTable: "ProductStages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StageResponses_ProductStages_StageId",
                table: "StageResponses");

            migrationBuilder.AddForeignKey(
                name: "FK_StageResponses_ProductStages_StageId",
                table: "StageResponses",
                column: "StageId",
                principalTable: "ProductStages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
