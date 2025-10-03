using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPicTweak.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateproperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLogs_Accounts_UserLogId",
                schema: "Logs",
                table: "UserLogs");

            migrationBuilder.AddColumn<Guid>(
                name: "GuestAccountId",
                schema: "Logs",
                table: "UserLogs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.DropColumn(
                name: "GuestAccountId",
                schema: "Logs",
                table: "UserLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogs_Accounts_UserLogId",
                schema: "Logs",
                table: "UserLogs",
                column: "UserLogId",
                principalSchema: "Accounts",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
