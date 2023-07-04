using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IceCreamBE.Migrations
{
    /// <inheritdoc />
    public partial class updateRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recipe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    img = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipe", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AccountDetail",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "ExpirationDate", "ExtensionDate" },
                values: new object[] { new DateTime(2023, 7, 4, 17, 21, 35, 93, DateTimeKind.Local).AddTicks(3784), new DateTime(2023, 7, 14, 17, 21, 35, 93, DateTimeKind.Local).AddTicks(3796), new DateTime(2023, 7, 4, 17, 21, 35, 93, DateTimeKind.Local).AddTicks(3812) });

            migrationBuilder.UpdateData(
                table: "Bill",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderTime",
                value: new DateTime(2023, 7, 4, 17, 21, 35, 93, DateTimeKind.Local).AddTicks(3865));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recipe");

            migrationBuilder.UpdateData(
                table: "AccountDetail",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "ExpirationDate", "ExtensionDate" },
                values: new object[] { new DateTime(2023, 7, 4, 17, 18, 57, 983, DateTimeKind.Local).AddTicks(882), new DateTime(2023, 7, 14, 17, 18, 57, 983, DateTimeKind.Local).AddTicks(895), new DateTime(2023, 7, 4, 17, 18, 57, 983, DateTimeKind.Local).AddTicks(910) });

            migrationBuilder.UpdateData(
                table: "Bill",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderTime",
                value: new DateTime(2023, 7, 4, 17, 18, 57, 983, DateTimeKind.Local).AddTicks(974));
        }
    }
}
