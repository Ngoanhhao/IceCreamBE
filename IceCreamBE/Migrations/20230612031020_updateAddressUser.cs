using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IceCreamBE.Migrations
{
    /// <inheritdoc />
    public partial class updateAddressUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AccountDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AccountDetail",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Address", "CreateDate", "ExpirationDate", "ExtensionDate" },
                values: new object[] { "Hội An", new DateTime(2023, 6, 12, 10, 10, 20, 95, DateTimeKind.Local).AddTicks(4462), new DateTime(2023, 6, 22, 10, 10, 20, 95, DateTimeKind.Local).AddTicks(4473), new DateTime(2023, 6, 12, 10, 10, 20, 95, DateTimeKind.Local).AddTicks(4488) });

            migrationBuilder.UpdateData(
                table: "Bill",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderTime",
                value: new DateTime(2023, 6, 12, 10, 10, 20, 95, DateTimeKind.Local).AddTicks(4544));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AccountDetail");

            migrationBuilder.UpdateData(
                table: "AccountDetail",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "ExpirationDate", "ExtensionDate" },
                values: new object[] { new DateTime(2023, 5, 22, 15, 12, 25, 681, DateTimeKind.Local).AddTicks(1804), new DateTime(2023, 6, 1, 15, 12, 25, 681, DateTimeKind.Local).AddTicks(1820), new DateTime(2023, 5, 22, 15, 12, 25, 681, DateTimeKind.Local).AddTicks(1845) });

            migrationBuilder.UpdateData(
                table: "Bill",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderTime",
                value: new DateTime(2023, 5, 22, 15, 12, 25, 681, DateTimeKind.Local).AddTicks(1949));
        }
    }
}
