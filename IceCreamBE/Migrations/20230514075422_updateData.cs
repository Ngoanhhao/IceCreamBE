using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IceCreamBE.Migrations
{
    /// <inheritdoc />
    public partial class updateData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AccountDetail",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ExpirationDate", "ExtensionDate" },
                values: new object[] { new DateTime(2023, 5, 24, 14, 54, 22, 10, DateTimeKind.Local).AddTicks(6376), new DateTime(2023, 5, 14, 14, 54, 22, 10, DateTimeKind.Local).AddTicks(6402) });

            migrationBuilder.UpdateData(
                table: "Bill",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderTime",
                value: new DateTime(2023, 5, 14, 14, 54, 22, 10, DateTimeKind.Local).AddTicks(6462));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AccountDetail",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ExpirationDate", "ExtensionDate" },
                values: new object[] { new DateTime(2023, 5, 24, 14, 49, 13, 289, DateTimeKind.Local).AddTicks(4715), new DateTime(2023, 5, 14, 14, 49, 13, 289, DateTimeKind.Local).AddTicks(4749) });

            migrationBuilder.UpdateData(
                table: "Bill",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderTime",
                value: new DateTime(2023, 5, 14, 14, 49, 13, 289, DateTimeKind.Local).AddTicks(4817));
        }
    }
}
