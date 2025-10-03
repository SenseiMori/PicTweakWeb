using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPicTweak.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class movetologsscheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Logs");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users",
                newSchema: "Logs");

            migrationBuilder.RenameTable(
                name: "SessionLogs",
                newName: "SessionLogs",
                newSchema: "Logs");

            migrationBuilder.RenameTable(
                name: "ImageLogs",
                newName: "ImageLogs",
                newSchema: "Logs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Users",
                schema: "Logs",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "SessionLogs",
                schema: "Logs",
                newName: "SessionLogs");

            migrationBuilder.RenameTable(
                name: "ImageLogs",
                schema: "Logs",
                newName: "ImageLogs");
        }
    }
}
