using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MBS_COMMAND.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTranModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Slots_ScheduleId",
                table: "Transactions");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Schedules_ScheduleId",
                table: "Transactions",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Schedules_ScheduleId",
                table: "Transactions");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Slots_ScheduleId",
                table: "Transactions",
                column: "ScheduleId",
                principalTable: "Slots",
                principalColumn: "Id");
        }
    }
}
