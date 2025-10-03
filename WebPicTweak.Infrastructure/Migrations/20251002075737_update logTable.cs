using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPicTweak.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatelogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionLogs_UserLogs_UserId",
                schema: "Logs",
                table: "SessionLogs");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "Logs",
                table: "SessionLogs",
                newName: "UserLogId");

            migrationBuilder.RenameIndex(
                name: "IX_SessionLogs_UserId",
                schema: "Logs",
                table: "SessionLogs",
                newName: "IX_SessionLogs_UserLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionLogs_UserLogs_UserLogId",
                schema: "Logs",
                table: "SessionLogs",
                column: "UserLogId",
                principalSchema: "Logs",
                principalTable: "UserLogs",
                principalColumn: "UserLogId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionLogs_UserLogs_UserLogId",
                schema: "Logs",
                table: "SessionLogs");

            migrationBuilder.RenameColumn(
                name: "UserLogId",
                schema: "Logs",
                table: "SessionLogs",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SessionLogs_UserLogId",
                schema: "Logs",
                table: "SessionLogs",
                newName: "IX_SessionLogs_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionLogs_UserLogs_UserId",
                schema: "Logs",
                table: "SessionLogs",
                column: "UserId",
                principalSchema: "Logs",
                principalTable: "UserLogs",
                principalColumn: "UserLogId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
