using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.App.API.Migrations
{
    /// <inheritdoc />
    public partial class removesupervisionhistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProspectionHistories");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Prospects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Prospects");

            migrationBuilder.CreateTable(
                name: "ProspectionHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProspectProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProspectCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProspectionHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProspectionHistories_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ProspectionHistories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProspectionHistories_Prospects_ProspectProductId_ProspectCompanyId",
                        columns: x => new { x.ProspectProductId, x.ProspectCompanyId },
                        principalTable: "Prospects",
                        principalColumns: new[] { "ProductId", "CompanyId" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProspectionHistories_ProspectProductId_ProspectCompanyId",
                table: "ProspectionHistories",
                columns: new[] { "ProspectProductId", "ProspectCompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProspectionHistories_UpdatedById",
                table: "ProspectionHistories",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProspectionHistories_UserId",
                table: "ProspectionHistories",
                column: "UserId");
        }
    }
}
