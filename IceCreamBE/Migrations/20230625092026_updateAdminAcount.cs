using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IceCreamBE.Migrations
{
    /// <inheritdoc />
    public partial class updateAdminAcount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AccountDetail",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "ExpirationDate", "ExtensionDate" },
                values: new object[] { new DateTime(2023, 6, 25, 16, 20, 25, 923, DateTimeKind.Local).AddTicks(8740), new DateTime(2023, 7, 5, 16, 20, 25, 923, DateTimeKind.Local).AddTicks(8753), new DateTime(2023, 6, 25, 16, 20, 25, 923, DateTimeKind.Local).AddTicks(8779) });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "21232f297a57a5a743894a0e4a801fc3");

            migrationBuilder.UpdateData(
                table: "Bill",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderTime",
                value: new DateTime(2023, 6, 25, 16, 20, 25, 923, DateTimeKind.Local).AddTicks(8861));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AccountDetail",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "ExpirationDate", "ExtensionDate" },
                values: new object[] { new DateTime(2023, 6, 24, 17, 4, 5, 349, DateTimeKind.Local).AddTicks(6464), new DateTime(2023, 7, 4, 17, 4, 5, 349, DateTimeKind.Local).AddTicks(6483), new DateTime(2023, 6, 24, 17, 4, 5, 349, DateTimeKind.Local).AddTicks(6511) });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "Admin");

            migrationBuilder.UpdateData(
                table: "Bill",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderTime",
                value: new DateTime(2023, 6, 24, 17, 4, 5, 349, DateTimeKind.Local).AddTicks(6678));
        }
    }
}
