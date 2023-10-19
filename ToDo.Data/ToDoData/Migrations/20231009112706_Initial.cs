using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CatalogValues");

            migrationBuilder.EnsureSchema(
                name: "LiveData");

            migrationBuilder.CreateTable(
                name: "Category",
                schema: "CatalogValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Bezeichnung = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "ToDoItem",
                schema: "LiveData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Bezeichnung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ImportanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToDoItem_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "CatalogValues",
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ToDoItem_Importance_ImportanceId",
                        column: x => x.ImportanceId,
                        principalSchema: "CatalogValues",
                        principalTable: "Importance",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ToDoItem_ToDoItem_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "LiveData",
                        principalTable: "ToDoItem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItem_CategoryId",
                schema: "LiveData",
                table: "ToDoItem",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItem_ImportanceId",
                schema: "LiveData",
                table: "ToDoItem",
                column: "ImportanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItem_ParentId",
                schema: "LiveData",
                table: "ToDoItem",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToDoItem",
                schema: "LiveData");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "CatalogValues");

            migrationBuilder.DropTable(
                name: "Importance",
                schema: "CatalogValues");
        }
    }
}
