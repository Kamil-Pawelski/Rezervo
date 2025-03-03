using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class SlotsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slot_Schedules_ScheduleId",
                table: "Slot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Slot",
                table: "Slot");

            migrationBuilder.RenameTable(
                name: "Slot",
                newName: "Slots");

            migrationBuilder.RenameIndex(
                name: "IX_Slot_ScheduleId",
                table: "Slots",
                newName: "IX_Slots_ScheduleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Slots",
                table: "Slots",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Slots_Schedules_ScheduleId",
                table: "Slots",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slots_Schedules_ScheduleId",
                table: "Slots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Slots",
                table: "Slots");

            migrationBuilder.RenameTable(
                name: "Slots",
                newName: "Slot");

            migrationBuilder.RenameIndex(
                name: "IX_Slots_ScheduleId",
                table: "Slot",
                newName: "IX_Slot_ScheduleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Slot",
                table: "Slot",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Slot_Schedules_ScheduleId",
                table: "Slot",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
