using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Category_Personalized : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ListId",
                schema: "CatalogValues",
                table: "Category",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "CatalogValues",
                table: "Category",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListId",
                schema: "CatalogValues",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "CatalogValues",
                table: "Category");
        }
    }
}
