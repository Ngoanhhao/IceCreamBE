using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IceCreamBE.Migrations
{
    /// <inheritdoc />
    public partial class repair : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "BillDetail",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Bill",
                type: "bit",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<double>(
                name: "SubTotal",
                table: "Bill",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Password", "Username" },
                values: new object[] { 1, "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "BrandName" },
                values: new object[] { 1, "Pepsi" });

            migrationBuilder.InsertData(
                table: "AccountDetail",
                columns: new[] { "Id", "Avatar", "Email", "ExpirationDate", "ExtensionDate", "FullName", "PhoneNumber", "RoleID" },
                values: new object[] { 1, null, "ngoanhhao24@gmail.com", new DateTime(2023, 5, 21, 16, 4, 49, 269, DateTimeKind.Local).AddTicks(5149), new DateTime(2023, 5, 11, 16, 4, 49, 269, DateTimeKind.Local).AddTicks(5175), "Ngô Anh Hào", "1234567890", 1 });

            migrationBuilder.InsertData(
                table: "Bill",
                columns: new[] { "Id", "AccountID", "BillDetailID", "OrderTime", "Status", "SubTotal", "Total", "VoucherID" },
                values: new object[] { 1, 1, 1, new DateTime(2023, 5, 11, 16, 4, 49, 269, DateTimeKind.Local).AddTicks(5234), true, 15000.0, 15000.0, null });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "BrandID", "Cost", "Description", "Discount", "Img", "Name", "Price", "Status", "Total" },
                values: new object[] { 1, 1, 10000.0, null, null, null, "Pepsi", 15000.0, true, 15000.0 });

            migrationBuilder.InsertData(
                table: "BillDetail",
                columns: new[] { "Id", "BillID", "Price", "ProductID", "Quantity", "Total" },
                values: new object[] { 1, 1, 15000.0, 1, 2, 30000.0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccountDetail",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BillDetail",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Bill",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Price",
                table: "BillDetail");

            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "Bill");

            migrationBuilder.AlterColumn<double>(
                name: "Status",
                table: "Bill",
                type: "float",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
