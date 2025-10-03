using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPicTweak.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class renameusertouserlogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageLogs_Users_UserId",
                schema: "Logs",
                table: "ImageLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionLogs_Users_UserId",
                schema: "Logs",
                table: "SessionLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                schema: "Logs",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "Logs",
                newName: "UserLogs",
                newSchema: "Logs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLogs",
                schema: "Logs",
                table: "UserLogs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageLogs_UserLogs_UserId",
                schema: "Logs",
                table: "ImageLogs",
                column: "UserId",
                principalSchema: "Logs",
                principalTable: "UserLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionLogs_UserLogs_UserId",
                schema: "Logs",
                table: "SessionLogs",
                column: "UserId",
                principalSchema: "Logs",
                principalTable: "UserLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageLogs_UserLogs_UserId",
                schema: "Logs",
                table: "ImageLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionLogs_UserLogs_UserId",
                schema: "Logs",
                table: "SessionLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLogs",
                schema: "Logs",
                table: "UserLogs");

            migrationBuilder.RenameTable(
                name: "UserLogs",
                schema: "Logs",
                newName: "Users",
                newSchema: "Logs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                schema: "Logs",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageLogs_Users_UserId",
                schema: "Logs",
                table: "ImageLogs",
                column: "UserId",
                principalSchema: "Logs",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionLogs_Users_UserId",
                schema: "Logs",
                table: "SessionLogs",
                column: "UserId",
                principalSchema: "Logs",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
