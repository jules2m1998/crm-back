using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.App.API.Migrations
{
    /// <inheritdoc />
    public partial class AddStageLevelToProductStage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFirst",
                table: "ProductStages");

            migrationBuilder.AddColumn<int>(
                name: "StageLevel",
                table: "ProductStages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StageLevel",
                table: "ProductStages");

            migrationBuilder.AddColumn<bool>(
                name: "IsFirst",
                table: "ProductStages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
