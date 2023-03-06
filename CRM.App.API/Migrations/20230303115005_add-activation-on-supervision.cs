using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.App.API.Migrations
{
    /// <inheritdoc />
    public partial class addactivationonsupervision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SupervisionHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SupervisionHistories");
        }
    }
}
