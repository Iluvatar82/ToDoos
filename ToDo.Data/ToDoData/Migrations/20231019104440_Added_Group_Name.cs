using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Added_Group_Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "LiveValues",
                table: "UserGroup",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoList_UserGroup_GroupId",
                schema: "LiveValues",
                table: "ToDoList");

            migrationBuilder.DropIndex(
                name: "IX_ToDoList_GroupId",
                schema: "LiveValues",
                table: "ToDoList");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "LiveValues",
                table: "UserGroup");
        }
    }
}
