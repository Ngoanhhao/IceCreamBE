﻿// <auto-generated />
using System;
using IceCreamBE.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IceCreamBE.Migrations
{
    [DbContext(typeof(IceCreamDbcontext))]
    [Migration("20230623143827_removeFeedbackRelationship")]
    partial class removeFeedbackRelationship
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("IceCreamBE.Models.AccountDetail", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExtensionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<int>("RoleID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleID");

                    b.ToTable("AccountDetail", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "Hội An",
                            CreateDate = new DateTime(2023, 6, 23, 21, 38, 27, 127, DateTimeKind.Local).AddTicks(3396),
                            Email = "ngoanhhao24@gmail.com",
                            ExpirationDate = new DateTime(2023, 7, 3, 21, 38, 27, 127, DateTimeKind.Local).AddTicks(3409),
                            ExtensionDate = new DateTime(2023, 6, 23, 21, 38, 27, 127, DateTimeKind.Local).AddTicks(3424),
                            FullName = "Ngô Anh Hào",
                            PhoneNumber = "1234567890",
                            RoleID = 1
                        });
                });

            modelBuilder.Entity("IceCreamBE.Models.Accounts", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Accounts", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Password = "Admin",
                            Username = "Admin"
                        });
                });

            modelBuilder.Entity("IceCreamBE.Models.Bill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountID")
                        .HasColumnType("int");

                    b.Property<DateTime>("OrderTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("SubTotal")
                        .HasColumnType("float");

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.Property<int?>("VoucherID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountID");

                    b.HasIndex("VoucherID");

                    b.ToTable("Bill", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AccountID = 1,
                            OrderTime = new DateTime(2023, 6, 23, 21, 38, 27, 127, DateTimeKind.Local).AddTicks(3489),
                            Status = "DONE",
                            SubTotal = 30000.0,
                            Total = 30000.0
                        });
                });

            modelBuilder.Entity("IceCreamBE.Models.BillDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BillID")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("BillID");

                    b.HasIndex("ProductID");

                    b.ToTable("BillDetail", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BillID = 1,
                            Price = 15000.0,
                            ProductID = 1,
                            Quantity = 2,
                            Total = 30000.0
                        });
                });

            modelBuilder.Entity("IceCreamBE.Models.Brands", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BrandName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Brands");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BrandName = "Pepsi"
                        });
                });

            modelBuilder.Entity("IceCreamBE.Models.Feedback", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FeedBackProduct")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Feedback", (string)null);
                });

            modelBuilder.Entity("IceCreamBE.Models.Products", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BrandID")
                        .HasColumnType("int");

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Discount")
                        .HasColumnType("int");

                    b.Property<string>("Img")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("BrandID");

                    b.ToTable("Products", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BrandID = 1,
                            Cost = 10000.0,
                            Name = "Pepsi",
                            Price = 15000.0,
                            Status = true,
                            Total = 15000.0
                        });
                });

            modelBuilder.Entity("IceCreamBE.Models.Recipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Recipe", (string)null);
                });

            modelBuilder.Entity("IceCreamBE.Models.RefreshToken", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("accessToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("createDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("expirationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("isUsed")
                        .HasColumnType("bit");

                    b.Property<string>("refreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("userId");

                    b.ToTable("RefreshToken", (string)null);
                });

            modelBuilder.Entity("IceCreamBE.Models.ResponseCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("ResponseCode");
                });

            modelBuilder.Entity("IceCreamBE.Models.Roles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Role = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            Role = "Member"
                        },
                        new
                        {
                            Id = 3,
                            Role = "Guest"
                        });
                });

            modelBuilder.Entity("IceCreamBE.Models.Storage", b =>
                {
                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastOrder")
                        .HasColumnType("datetime2");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("ProductID");

                    b.ToTable("Storage", (string)null);
                });

            modelBuilder.Entity("IceCreamBE.Models.Vouchers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AdminID")
                        .HasColumnType("int");

                    b.Property<int>("Discount")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("Voucher")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AdminID");

                    b.ToTable("Voucher", (string)null);
                });

            modelBuilder.Entity("IceCreamBE.Models.AccountDetail", b =>
                {
                    b.HasOne("IceCreamBE.Models.Accounts", "Accounts")
                        .WithOne("AccountDetail")
                        .HasForeignKey("IceCreamBE.Models.AccountDetail", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IceCreamBE.Models.Roles", "Role")
                        .WithMany("AccountDetail")
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Accounts");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("IceCreamBE.Models.Bill", b =>
                {
                    b.HasOne("IceCreamBE.Models.Accounts", "Account")
                        .WithMany("Bill")
                        .HasForeignKey("AccountID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IceCreamBE.Models.Vouchers", "Vouchers")
                        .WithMany("Bill")
                        .HasForeignKey("VoucherID")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Account");

                    b.Navigation("Vouchers");
                });

            modelBuilder.Entity("IceCreamBE.Models.BillDetail", b =>
                {
                    b.HasOne("IceCreamBE.Models.Bill", "Bill")
                        .WithMany("BillDetail")
                        .HasForeignKey("BillID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IceCreamBE.Models.Products", "Product")
                        .WithMany("Details")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bill");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("IceCreamBE.Models.Products", b =>
                {
                    b.HasOne("IceCreamBE.Models.Brands", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");
                });

            modelBuilder.Entity("IceCreamBE.Models.Recipe", b =>
                {
                    b.HasOne("IceCreamBE.Models.Products", "Product")
                        .WithMany("Recipe")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("IceCreamBE.Models.RefreshToken", b =>
                {
                    b.HasOne("IceCreamBE.Models.Accounts", "user")
                        .WithMany("RefreshToken")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("IceCreamBE.Models.Storage", b =>
                {
                    b.HasOne("IceCreamBE.Models.Products", "Product")
                        .WithOne("Storage")
                        .HasForeignKey("IceCreamBE.Models.Storage", "ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("IceCreamBE.Models.Vouchers", b =>
                {
                    b.HasOne("IceCreamBE.Models.Accounts", "Admin")
                        .WithMany("vouchers")
                        .HasForeignKey("AdminID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("IceCreamBE.Models.Accounts", b =>
                {
                    b.Navigation("AccountDetail")
                        .IsRequired();

                    b.Navigation("Bill");

                    b.Navigation("RefreshToken");

                    b.Navigation("vouchers");
                });

            modelBuilder.Entity("IceCreamBE.Models.Bill", b =>
                {
                    b.Navigation("BillDetail");
                });

            modelBuilder.Entity("IceCreamBE.Models.Products", b =>
                {
                    b.Navigation("Details");

                    b.Navigation("Recipe");

                    b.Navigation("Storage")
                        .IsRequired();
                });

            modelBuilder.Entity("IceCreamBE.Models.Roles", b =>
                {
                    b.Navigation("AccountDetail");
                });

            modelBuilder.Entity("IceCreamBE.Models.Vouchers", b =>
                {
                    b.Navigation("Bill");
                });
#pragma warning restore 612, 618
        }
    }
}