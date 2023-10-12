using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Done_Changed_Importance_Removed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoItem_Importance_ImportanceId",
                schema: "LiveData",
                table: "ToDoItem");

            migrationBuilder.DropTable(
                name: "Importance",
                schema: "CatalogValues");

            migrationBuilder.DropIndex(
                name: "IX_ToDoItem_ImportanceId",
                schema: "LiveData",
                table: "ToDoItem");

            migrationBuilder.DropColumn(
                name: "ImportanceId",
                schema: "LiveData",
                table: "ToDoItem");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Done",
                schema: "LiveData",
                table: "ToDoItem",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Done",
                schema: "LiveData",
                table: "ToDoItem",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ImportanceId",
                schema: "LiveData",
                table: "ToDoItem",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Importance",
                schema: "CatalogValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Bezeichnung = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Importance", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItem_ImportanceId",
                schema: "LiveData",
                table: "ToDoItem",
                column: "ImportanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoItem_Importance_ImportanceId",
                schema: "LiveData",
                table: "ToDoItem",
                column: "ImportanceId",
                principalSchema: "CatalogValues",
                principalTable: "Importance",
                principalColumn: "Id");
        }
    }
}
