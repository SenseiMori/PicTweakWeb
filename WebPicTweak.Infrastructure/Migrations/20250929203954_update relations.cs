using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPicTweak.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updaterelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageLogs_UserLogs_UserId",
                schema: "Logs",
                table: "ImageLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLogs_Accounts_Id",
                schema: "Logs",
                table: "UserLogs");

            migrationBuilder.DropIndex(
                name: "IX_ImageLogs_UserId",
                schema: "Logs",
                table: "ImageLogs");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "Logs",
                table: "UserLogs");

            migrationBuilder.DropColumn(
                name: "Nickname",
                schema: "Logs",
                table: "UserLogs");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "Logs",
                table: "ImageLogs");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "Logs",
                table: "UserLogs",
                newName: "UserLogId");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                schema: "Logs",
                table: "UserLogs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLogs_Accounts_UserLogId",
                schema: "Logs",
                table: "UserLogs");

            migrationBuilder.DropColumn(
                name: "AccountId",
                schema: "Logs",
                table: "UserLogs");

            migrationBuilder.RenameColumn(
                name: "UserLogId",
                schema: "Logs",
                table: "UserLogs",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "Logs",
                table: "UserLogs",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nickname",
                schema: "Logs",
                table: "UserLogs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "Logs",
                table: "ImageLogs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ImageLogs_UserId",
                schema: "Logs",
                table: "ImageLogs",
                column: "UserId");

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
                name: "FK_UserLogs_Accounts_Id",
                schema: "Logs",
                table: "UserLogs",
                column: "Id",
                principalSchema: "Accounts",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
