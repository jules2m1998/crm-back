using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.App.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletedAtOnProspect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Prospects",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Prospects");
        }
    }
}
