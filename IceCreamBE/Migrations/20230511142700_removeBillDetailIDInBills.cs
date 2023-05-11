using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IceCreamBE.Migrations
{
    /// <inheritdoc />
    public partial class removeBillDetailIDInBills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillDetailID",
                table: "Bill");

            migrationBuilder.UpdateData(
                table: "AccountDetail",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ExpirationDate", "ExtensionDate" },
                values: new object[] { new DateTime(2023, 5, 21, 21, 27, 0, 446, DateTimeKind.Local).AddTicks(7), new DateTime(2023, 5, 11, 21, 27, 0, 446, DateTimeKind.Local).AddTicks(48) });

            migrationBuilder.UpdateData(
                table: "Bill",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderTime",
                value: new DateTime(2023, 5, 11, 21, 27, 0, 446, DateTimeKind.Local).AddTicks(143));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BillDetailID",
                table: "Bill",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AccountDetail",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ExpirationDate", "ExtensionDate" },
                values: new object[] { new DateTime(2023, 5, 21, 16, 4, 49, 269, DateTimeKind.Local).AddTicks(5149), new DateTime(2023, 5, 11, 16, 4, 49, 269, DateTimeKind.Local).AddTicks(5175) });

            migrationBuilder.UpdateData(
                table: "Bill",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BillDetailID", "OrderTime" },
                values: new object[] { 1, new DateTime(2023, 5, 11, 16, 4, 49, 269, DateTimeKind.Local).AddTicks(5234) });
        }
    }
}
