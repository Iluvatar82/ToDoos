using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Align_Schema_Names : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "UserGroup",
                schema: "LiveValues",
                newName: "UserGroup",
                newSchema: "LiveData");

            migrationBuilder.RenameTable(
                name: "ToDoList",
                schema: "LiveValues",
                newName: "ToDoList",
                newSchema: "LiveData");

            migrationBuilder.RenameTable(
                name: "Category",
                schema: "CatalogValues",
                newName: "Category",
                newSchema: "LiveData");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CatalogValues");

            migrationBuilder.EnsureSchema(
                name: "LiveValues");

            migrationBuilder.RenameTable(
                name: "UserGroup",
                schema: "LiveData",
                newName: "UserGroup",
                newSchema: "LiveValues");

            migrationBuilder.RenameTable(
                name: "ToDoList",
                schema: "LiveData",
                newName: "ToDoList",
                newSchema: "LiveValues");

            migrationBuilder.RenameTable(
                name: "Category",
                schema: "LiveData",
                newName: "Category",
                newSchema: "CatalogValues");
        }
    }
}
