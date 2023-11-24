using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Data.Migrations
{
    /// <inheritdoc />
    public partial class IsActive_to_InactiveSince : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InactiveSince",
                schema: "LiveData",
                table: "ToDoItem",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InactiveSince",
                schema: "LiveData",
                table: "ToDoItem");
        }
    }
}
