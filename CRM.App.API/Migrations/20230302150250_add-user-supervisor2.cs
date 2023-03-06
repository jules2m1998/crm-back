using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.App.API.Migrations
{
    /// <inheritdoc />
    public partial class addusersupervisor2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_SupervisorId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SupervisorId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SupervisorId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "SupervisionHistories",
                columns: table => new
                {
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupervisedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupervisorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupervisionHistories", x => new { x.SupervisedId, x.SupervisorId, x.CreatedAt });
                    table.ForeignKey(
                        name: "FK_SupervisionHistories_AspNetUsers_SupervisedId",
                        column: x => x.SupervisedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupervisionHistories_AspNetUsers_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupervisionHistories_SupervisorId",
                table: "SupervisionHistories",
                column: "SupervisorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupervisionHistories");

            migrationBuilder.AddColumn<Guid>(
                name: "SupervisorId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SupervisorId",
                table: "AspNetUsers",
                column: "SupervisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_SupervisorId",
                table: "AspNetUsers",
                column: "SupervisorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
