using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.App.API.Migrations
{
    /// <inheritdoc />
    public partial class Solved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Events_EventId",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_EventId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Contacts");

            migrationBuilder.CreateTable(
                name: "ContactEvent",
                columns: table => new
                {
                    ContactId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactEvent", x => new { x.ContactId, x.EventsId });
                    table.ForeignKey(
                        name: "FK_ContactEvent_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactEvent_Events_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactEvent_EventsId",
                table: "ContactEvent",
                column: "EventsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactEvent");

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Contacts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_EventId",
                table: "Contacts",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Events_EventId",
                table: "Contacts",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }
    }
}
