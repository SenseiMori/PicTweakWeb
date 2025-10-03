using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPicTweak.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addguestaccountstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GuestAccounts",
                schema: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EntryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestAccounts", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogs_GuestAccounts_UserLogId",
                schema: "Logs",
                table: "UserLogs",
                column: "UserLogId",
                principalSchema: "Accounts",
                principalTable: "GuestAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLogs_GuestAccounts_UserLogId",
                schema: "Logs",
                table: "UserLogs");

            migrationBuilder.DropTable(
                name: "GuestAccounts",
                schema: "Accounts");
        }
    }
}
