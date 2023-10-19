using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Changes_Foreign_Key : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoItem_ToDoList_ListId",
                schema: "LiveData",
                table: "ToDoItem");

            migrationBuilder.DropIndex(
                name: "IX_ToDoItem_ListId",
                schema: "LiveData",
                table: "ToDoItem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ToDoItem_ListId",
                schema: "LiveData",
                table: "ToDoItem",
                column: "ListId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoItem_ToDoList_ListId",
                schema: "LiveData",
                table: "ToDoItem",
                column: "ListId",
                principalSchema: "LiveValues",
                principalTable: "ToDoList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
