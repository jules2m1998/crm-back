using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.App.API.Migrations
{
    /// <inheritdoc />
    public partial class AddEventEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_phoneNumbers_Contacts_ContactId",
                table: "phoneNumbers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_phoneNumbers",
                table: "phoneNumbers");

            migrationBuilder.RenameTable(
                name: "phoneNumbers",
                newName: "PhoneNumbers");

            migrationBuilder.RenameIndex(
                name: "IX_phoneNumbers_ContactId",
                table: "PhoneNumbers",
                newName: "IX_PhoneNumbers_ContactId");

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Contacts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhoneNumbers",
                table: "PhoneNumbers",
                column: "Value");

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProspectProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProspectCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActivated = table.Column<bool>(type: "bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Events_Prospects_ProspectProductId_ProspectCompanyId",
                        columns: x => new { x.ProspectProductId, x.ProspectCompanyId },
                        principalTable: "Prospects",
                        principalColumns: new[] { "ProductId", "CompanyId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_EventId",
                table: "Contacts",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_CreatorId",
                table: "Events",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_ProspectProductId_ProspectCompanyId",
                table: "Events",
                columns: new[] { "ProspectProductId", "ProspectCompanyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Events_EventId",
                table: "Contacts",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PhoneNumbers_Contacts_ContactId",
                table: "PhoneNumbers",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Events_EventId",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_PhoneNumbers_Contacts_ContactId",
                table: "PhoneNumbers");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PhoneNumbers",
                table: "PhoneNumbers");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_EventId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Contacts");

            migrationBuilder.RenameTable(
                name: "PhoneNumbers",
                newName: "phoneNumbers");

            migrationBuilder.RenameIndex(
                name: "IX_PhoneNumbers_ContactId",
                table: "phoneNumbers",
                newName: "IX_phoneNumbers_ContactId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_phoneNumbers",
                table: "phoneNumbers",
                column: "Value");

            migrationBuilder.AddForeignKey(
                name: "FK_phoneNumbers_Contacts_ContactId",
                table: "phoneNumbers",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
