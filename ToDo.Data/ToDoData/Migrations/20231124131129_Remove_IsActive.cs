using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Remove_IsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "LiveData",
                table: "ToDoItem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "LiveData",
                table: "ToDoItem",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }
    }
}
