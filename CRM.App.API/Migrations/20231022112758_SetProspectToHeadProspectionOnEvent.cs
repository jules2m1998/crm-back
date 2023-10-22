using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.App.API.Migrations
{
    /// <inheritdoc />
    public partial class SetProspectToHeadProspectionOnEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Prospects_ProspectProductId_ProspectCompanyId",
                table: "Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HeadProspections",
                table: "HeadProspections");

            migrationBuilder.DropIndex(
                name: "IX_HeadProspections_AgentId_ProductId_CompanyId",
                table: "HeadProspections");

            migrationBuilder.DropIndex(
                name: "IX_Events_ProspectProductId_ProspectCompanyId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "HeadProspections");

            migrationBuilder.DropColumn(
                name: "ProspectCompanyId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ProspectProductId",
                table: "Events");

            migrationBuilder.AddColumn<Guid>(
                name: "AgentId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_HeadProspections",
                table: "HeadProspections",
                columns: new[] { "AgentId", "ProductId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_Events_AgentId_ProductId_CompanyId",
                table: "Events",
                columns: new[] { "AgentId", "ProductId", "CompanyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Events_HeadProspections_AgentId_ProductId_CompanyId",
                table: "Events",
                columns: new[] { "AgentId", "ProductId", "CompanyId" },
                principalTable: "HeadProspections",
                principalColumns: new[] { "AgentId", "ProductId", "CompanyId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_HeadProspections_AgentId_ProductId_CompanyId",
                table: "Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HeadProspections",
                table: "HeadProspections");

            migrationBuilder.DropIndex(
                name: "IX_Events_AgentId_ProductId_CompanyId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Events");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "HeadProspections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProspectCompanyId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProspectProductId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_HeadProspections",
                table: "HeadProspections",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_HeadProspections_AgentId_ProductId_CompanyId",
                table: "HeadProspections",
                columns: new[] { "AgentId", "ProductId", "CompanyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_ProspectProductId_ProspectCompanyId",
                table: "Events",
                columns: new[] { "ProspectProductId", "ProspectCompanyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Prospects_ProspectProductId_ProspectCompanyId",
                table: "Events",
                columns: new[] { "ProspectProductId", "ProspectCompanyId" },
                principalTable: "Prospects",
                principalColumns: new[] { "ProductId", "CompanyId" });
        }
    }
}
