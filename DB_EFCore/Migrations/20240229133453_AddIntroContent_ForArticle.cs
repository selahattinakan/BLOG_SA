using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB_EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddIntroContent_ForArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IntroContent",
                table: "Article",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntroContent",
                table: "Article");
        }
    }
}
