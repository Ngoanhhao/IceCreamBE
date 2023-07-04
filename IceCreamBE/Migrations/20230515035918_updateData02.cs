using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IceCreamBE.Migrations
{
    /// <inheritdoc />
    public partial class updateData02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
            name: "CreateDate",
            table: "AccountDetail",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
                columns: new[] { "OrderTime", "SubTotal", "Total" },
                values: new object[] { new DateTime(2023, 5, 15, 10, 59, 18, 205, DateTimeKind.Local).AddTicks(9939), 30000.0, 30000.0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "AccountDetail");

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
                columns: new[] { "OrderTime", "SubTotal", "Total" },
                values: new object[] { new DateTime(2023, 5, 11, 21, 27, 0, 446, DateTimeKind.Local).AddTicks(143), 15000.0, 15000.0 });
        }
    }
}
