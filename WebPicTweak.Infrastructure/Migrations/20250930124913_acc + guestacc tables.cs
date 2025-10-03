using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPicTweak.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class accguestacctables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLogs_GuestAccounts_UserLogId",
                schema: "Logs",
                table: "UserLogs");

            migrationBuilder.DropTable(
                name: "GuestAccounts",
                schema: "Accounts");

            migrationBuilder.DropColumn(
                name: "GuestAccountId",
                schema: "Logs",
                table: "UserLogs");

            migrationBuilder.AddColumn<string>(
                name: "AccountType",
                schema: "Accounts",
                table: "Accounts",
                type: "character varying(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EntryTime",
                schema: "Accounts",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountType",
                schema: "Accounts",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "EntryTime",
                schema: "Accounts",
                table: "Accounts");

            migrationBuilder.AddColumn<Guid>(
                name: "GuestAccountId",
                schema: "Logs",
                table: "UserLogs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
    }
}
