using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IceCreamBE.Migrations
{
    /// <inheritdoc />
    public partial class updateResponcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResponseCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponseCode", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AccountDetail",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "ExpirationDate", "ExtensionDate" },
                values: new object[] { new DateTime(2023, 5, 15, 11, 12, 52, 680, DateTimeKind.Local).AddTicks(4792), new DateTime(2023, 5, 25, 11, 12, 52, 680, DateTimeKind.Local).AddTicks(4803), new DateTime(2023, 5, 15, 11, 12, 52, 680, DateTimeKind.Local).AddTicks(4817) });

            migrationBuilder.UpdateData(
                table: "Bill",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderTime",
                value: new DateTime(2023, 5, 15, 11, 12, 52, 680, DateTimeKind.Local).AddTicks(4876));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResponseCode");

            migrationBuilder.UpdateData(
                table: "AccountDetail",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "ExpirationDate", "ExtensionDate" },
                values: new object[] { new DateTime(2023, 5, 15, 10, 59, 18, 205, DateTimeKind.Local).AddTicks(9805), new DateTime(2023, 5, 25, 10, 59, 18, 205, DateTimeKind.Local).AddTicks(9821), new DateTime(2023, 5, 15, 10, 59, 18, 205, DateTimeKind.Local).AddTicks(9836) });

            migrationBuilder.UpdateData(
                table: "Bill",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderTime",
                value: new DateTime(2023, 5, 15, 10, 59, 18, 205, DateTimeKind.Local).AddTicks(9939));
        }
    }
}
