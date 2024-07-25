using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaApp.Migrations
{
    /// <inheritdoc />
    public partial class adjustGachatablealittlebit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "GachaItems",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "GachaItems",
                newName: "thumb_url");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "GachaItems",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "thumb_url",
                table: "GachaItems",
                newName: "Image");
        }
    }
}
