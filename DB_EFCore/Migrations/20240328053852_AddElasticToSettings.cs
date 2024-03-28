using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB_EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddElasticToSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsElasticsearchEnable",
                table: "Setting",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsElasticsearchEnable",
                table: "Setting");
        }
    }
}
