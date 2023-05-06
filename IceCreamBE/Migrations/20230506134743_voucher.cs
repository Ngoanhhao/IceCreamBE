using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IceCreamBE.Migrations
{
    /// <inheritdoc />
    public partial class voucher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Status",
                table: "Recipe",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "VoucherID",
                table: "Bill",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Voucher",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Voucher = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminID = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    //RoleID = table.Column<int>(type: "int", nullable: false),
                    //ProductIDList = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voucher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Voucher_Accounts_AdminID",
                        column: x => x.AdminID,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    //table.ForeignKey(
                    //    name: "FK_Voucher_Roles_RoleID",
                    //    column: x => x.RoleID,
                    //    principalTable: "Roles",
                    //    principalColumn: "Id",
                    //    onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bill_VoucherID",
                table: "Bill",
                column: "VoucherID");

            migrationBuilder.CreateIndex(
                name: "IX_Voucher_AdminID",
                table: "Voucher",
                column: "AdminID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Voucher_RoleID",
            //    table: "Voucher",
            //    column: "RoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bill_Voucher_VoucherID",
                table: "Bill",
                column: "VoucherID",
                principalTable: "Voucher",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bill_Voucher_VoucherID",
                table: "Bill");

            migrationBuilder.DropTable(
                name: "Voucher");

            migrationBuilder.DropIndex(
                name: "IX_Bill_VoucherID",
                table: "Bill");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "VoucherID",
                table: "Bill");
        }
    }
}
