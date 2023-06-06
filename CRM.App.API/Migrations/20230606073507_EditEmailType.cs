using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.App.API.Migrations
{
    /// <inheritdoc />
    public partial class EditEmailType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Body",
                table: "Emails");

            migrationBuilder.DropColumn(
                name: "ReceiverEmail",
                table: "Emails");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Emails");

            migrationBuilder.AddColumn<int>(
                name: "EmailType",
                table: "Emails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailType",
                table: "Emails");

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "Emails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverEmail",
                table: "Emails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Emails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
