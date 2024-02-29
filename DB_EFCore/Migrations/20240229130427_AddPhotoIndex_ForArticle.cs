using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB_EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotoIndex_ForArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PhotoIndex",
                table: "Article",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoIndex",
                table: "Article");
        }
    }
}
