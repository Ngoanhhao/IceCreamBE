using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IceCreamBE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationshipProductRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Recipe",
                table: "Recipe");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Recipe",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recipe",
                table: "Recipe",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_ProductId",
                table: "Recipe",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Recipe",
                table: "Recipe");

            migrationBuilder.DropIndex(
                name: "IX_Recipe_ProductId",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Recipe");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recipe",
                table: "Recipe",
                column: "ProductId");
        }
    }
}
