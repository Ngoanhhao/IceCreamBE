using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IceCreamBE.Migrations
{
    /// <inheritdoc />
    public partial class updateBillStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Bill",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.UpdateData(
                table: "AccountDetail",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "ExpirationDate", "ExtensionDate" },
                values: new object[] { new DateTime(2023, 6, 12, 22, 47, 31, 765, DateTimeKind.Local).AddTicks(667), new DateTime(2023, 6, 22, 22, 47, 31, 765, DateTimeKind.Local).AddTicks(678), new DateTime(2023, 6, 12, 22, 47, 31, 765, DateTimeKind.Local).AddTicks(693) });

            migrationBuilder.UpdateData(
                table: "Bill",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "OrderTime", "Status" },
                values: new object[] { new DateTime(2023, 6, 12, 22, 47, 31, 765, DateTimeKind.Local).AddTicks(755), "DONE" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Bill",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AccountDetail",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "ExpirationDate", "ExtensionDate" },
                values: new object[] { new DateTime(2023, 6, 12, 10, 10, 20, 95, DateTimeKind.Local).AddTicks(4462), new DateTime(2023, 6, 22, 10, 10, 20, 95, DateTimeKind.Local).AddTicks(4473), new DateTime(2023, 6, 12, 10, 10, 20, 95, DateTimeKind.Local).AddTicks(4488) });

            migrationBuilder.UpdateData(
                table: "Bill",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "OrderTime", "Status" },
                values: new object[] { new DateTime(2023, 6, 12, 10, 10, 20, 95, DateTimeKind.Local).AddTicks(4544), true });
        }
    }
}
