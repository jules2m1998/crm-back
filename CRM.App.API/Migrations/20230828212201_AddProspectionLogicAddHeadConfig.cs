using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.App.API.Migrations
{
    /// <inheritdoc />
    public partial class AddProspectionLogicAddHeadConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HeadProspections_AgentId",
                table: "HeadProspections");

            migrationBuilder.CreateIndex(
                name: "IX_HeadProspections_AgentId_ProductId_CompanyId",
                table: "HeadProspections",
                columns: new[] { "AgentId", "ProductId", "CompanyId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HeadProspections_AgentId_ProductId_CompanyId",
                table: "HeadProspections");

            migrationBuilder.CreateIndex(
                name: "IX_HeadProspections_AgentId",
                table: "HeadProspections",
                column: "AgentId");
        }
    }
}
