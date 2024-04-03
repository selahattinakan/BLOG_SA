using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB_EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddRedisAndBioToSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BioText",
                table: "Setting",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsRedisEnable",
                table: "Setting",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BioText",
                table: "Setting");

            migrationBuilder.DropColumn(
                name: "IsRedisEnable",
                table: "Setting");
        }
    }
}
