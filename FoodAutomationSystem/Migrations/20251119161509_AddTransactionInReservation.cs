using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodAutomationSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionInReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "Reservation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_TransactionId",
                table: "Reservation",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Transaction_TransactionId",
                table: "Reservation",
                column: "TransactionId",
                principalTable: "Transaction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Transaction_TransactionId",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_TransactionId",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Reservation");
        }
    }
}
