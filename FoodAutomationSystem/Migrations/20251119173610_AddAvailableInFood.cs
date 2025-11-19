using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodAutomationSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddAvailableInFood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Available",
                table: "Food",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Available",
                table: "Food");
        }
    }
}
