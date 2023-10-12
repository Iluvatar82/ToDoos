using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Change_Color : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Argb",
                schema: "CatalogValues",
                table: "Category");

            migrationBuilder.AddColumn<string>(
                name: "RGB_A",
                schema: "CatalogValues",
                table: "Category",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RGB_A",
                schema: "CatalogValues",
                table: "Category");

            migrationBuilder.AddColumn<int>(
                name: "Argb",
                schema: "CatalogValues",
                table: "Category",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
