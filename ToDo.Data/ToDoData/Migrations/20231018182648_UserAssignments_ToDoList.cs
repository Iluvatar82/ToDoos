using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserAssignments_ToDoList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserToDoAssignment",
                schema: "LiveData");

            migrationBuilder.EnsureSchema(
                name: "LiveValues");

            migrationBuilder.AddColumn<Guid>(
                name: "ListId",
                schema: "LiveData",
                table: "ToDoItem",
                type: "uniqueidentifier",
                nullable: true,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ToDoList",
                schema: "LiveValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoList", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoItem_ToDoList_ListId",
                schema: "LiveData",
                table: "ToDoItem");

            migrationBuilder.DropTable(
                name: "ToDoList",
                schema: "LiveValues");

            migrationBuilder.DropIndex(
                name: "IX_ToDoItem_ListId",
                schema: "LiveData",
                table: "ToDoItem");

            migrationBuilder.DropColumn(
                name: "ListId",
                schema: "LiveData",
                table: "ToDoItem");

            migrationBuilder.CreateTable(
                name: "UserToDoAssignment",
                schema: "LiveData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToDoItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToDoAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserToDoAssignment_ToDoItem_ToDoItemId",
                        column: x => x.ToDoItemId,
                        principalSchema: "LiveData",
                        principalTable: "ToDoItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserToDoAssignment_ToDoItemId",
                schema: "LiveData",
                table: "UserToDoAssignment",
                column: "ToDoItemId");
        }
    }
}
