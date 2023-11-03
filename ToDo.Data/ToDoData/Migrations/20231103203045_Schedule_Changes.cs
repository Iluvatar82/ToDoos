using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Schedule_Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_ToDoItem_ToDoItemId",
                schema: "LiveData",
                table: "Schedule");

            migrationBuilder.AlterColumn<Guid>(
                name: "ToDoItemId",
                schema: "LiveData",
                table: "Schedule",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_ToDoItem_ToDoItemId",
                schema: "LiveData",
                table: "Schedule",
                column: "ToDoItemId",
                principalSchema: "LiveData",
                principalTable: "ToDoItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_ToDoItem_ToDoItemId",
                schema: "LiveData",
                table: "Schedule");

            migrationBuilder.AlterColumn<Guid>(
                name: "ToDoItemId",
                schema: "LiveData",
                table: "Schedule",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_ToDoItem_ToDoItemId",
                schema: "LiveData",
                table: "Schedule",
                column: "ToDoItemId",
                principalSchema: "LiveData",
                principalTable: "ToDoItem",
                principalColumn: "Id");
        }
    }
}
