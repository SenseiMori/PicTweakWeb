using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPicTweak.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateAccountTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLogs_Accounts_AccountId",
                schema: "Logs",
                table: "UserLogs");

            migrationBuilder.DropIndex(
                name: "IX_UserLogs_AccountId",
                schema: "Logs",
                table: "UserLogs");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogs_AccountId",
                schema: "Logs",
                table: "UserLogs",
                column: "AccountId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogs_Accounts_AccountId",
                schema: "Logs",
                table: "UserLogs",
                column: "AccountId",
                principalSchema: "Accounts",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLogs_Accounts_AccountId",
                schema: "Logs",
                table: "UserLogs");

            migrationBuilder.DropIndex(
                name: "IX_UserLogs_AccountId",
                schema: "Logs",
                table: "UserLogs");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogs_AccountId",
                schema: "Logs",
                table: "UserLogs",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogs_Accounts_AccountId",
                schema: "Logs",
                table: "UserLogs",
                column: "AccountId",
                principalSchema: "Accounts",
                principalTable: "Accounts",
                principalColumn: "Id");
        }
    }
}
