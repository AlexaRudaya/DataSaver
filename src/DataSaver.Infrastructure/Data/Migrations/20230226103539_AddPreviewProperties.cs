using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataSaver.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPreviewProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreviewImage",
                table: "Links",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviewTitle",
                table: "Links",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviewImage",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "PreviewTitle",
                table: "Links");
        }
    }
}
