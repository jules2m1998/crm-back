using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.App.API.Migrations
{
    /// <inheritdoc />
    public partial class AddStagesToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductStages_FirstStageId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_FirstStageId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FirstStageId",
                table: "Products");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "ProductStages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductStages_ProductId",
                table: "ProductStages",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStages_Products_ProductId",
                table: "ProductStages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductStages_Products_ProductId",
                table: "ProductStages");

            migrationBuilder.DropIndex(
                name: "IX_ProductStages_ProductId",
                table: "ProductStages");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductStages");

            migrationBuilder.AddColumn<Guid>(
                name: "FirstStageId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_FirstStageId",
                table: "Products",
                column: "FirstStageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductStages_FirstStageId",
                table: "Products",
                column: "FirstStageId",
                principalTable: "ProductStages",
                principalColumn: "Id");
        }
    }
}
