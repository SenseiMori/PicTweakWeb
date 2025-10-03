using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPicTweak.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addsizebigintimagetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<long>(
                name: "Size",
                schema: "Logs",
                table: "ImageLogs",
                type: "bigint",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpiresAt",
                schema: "Logs",
                table: "SessionLogs",
                newName: "CompletedAt");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "Logs",
                table: "SessionLogs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Size",
                schema: "Logs",
                table: "ImageLogs",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldMaxLength: 50);
        }
    }
}
