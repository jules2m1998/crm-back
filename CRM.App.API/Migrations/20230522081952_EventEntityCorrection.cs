using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.App.API.Migrations
{
    /// <inheritdoc />
    public partial class EventEntityCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Prospects_ProspectProductId_ProspectCompanyId",
                table: "Events");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProspectProductId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProspectCompanyId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Prospects_ProspectProductId_ProspectCompanyId",
                table: "Events",
                columns: new[] { "ProspectProductId", "ProspectCompanyId" },
                principalTable: "Prospects",
                principalColumns: new[] { "ProductId", "CompanyId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Prospects_ProspectProductId_ProspectCompanyId",
                table: "Events");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProspectProductId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ProspectCompanyId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Prospects_ProspectProductId_ProspectCompanyId",
                table: "Events",
                columns: new[] { "ProspectProductId", "ProspectCompanyId" },
                principalTable: "Prospects",
                principalColumns: new[] { "ProductId", "CompanyId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
