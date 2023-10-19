using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Data.Migrations
{
    /// <inheritdoc />
    public partial class EntityChanges_Required_MinLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoItem_Category_CategoryId",
                schema: "LiveData",
                table: "ToDoItem");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                schema: "LiveData",
                table: "ToDoItem",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoItem_Category_CategoryId",
                schema: "LiveData",
                table: "ToDoItem",
                column: "CategoryId",
                principalSchema: "CatalogValues",
                principalTable: "Category",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoItem_Category_CategoryId",
                schema: "LiveData",
                table: "ToDoItem");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                schema: "LiveData",
                table: "ToDoItem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoItem_Category_CategoryId",
                schema: "LiveData",
                table: "ToDoItem",
                column: "CategoryId",
                principalSchema: "CatalogValues",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
