using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Schedules_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Schedule",
                schema: "LiveData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: true),
                    End = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ScheduleDefinition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToDoItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedule_ToDoItem_ToDoItemId",
                        column: x => x.ToDoItemId,
                        principalSchema: "LiveData",
                        principalTable: "ToDoItem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_ToDoItemId",
                schema: "LiveData",
                table: "Schedule",
                column: "ToDoItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedule",
                schema: "LiveData");
        }
    }
}
