using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Remove_FK_List_UserGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoList_UserGroup_GroupId",
                schema: "LiveValues",
                table: "ToDoList");

            migrationBuilder.DropIndex(
                name: "IX_ToDoList_GroupId",
                schema: "LiveValues",
                table: "ToDoList");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ToDoList_GroupId",
                schema: "LiveValues",
                table: "ToDoList",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoList_UserGroup_GroupId",
                schema: "LiveValues",
                table: "ToDoList",
                column: "GroupId",
                principalSchema: "LiveValues",
                principalTable: "UserGroup",
                principalColumn: "Id");
        }
    }
}
