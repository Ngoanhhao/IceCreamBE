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
    [Migration("20230506151359_UpdateRelationshipProduct-Recipe")]
    partial class UpdateRelationshipProductRecipe
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

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.HasKey("Id");

                    b.ToTable("AccountDetail", (string)null);
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

                    b.Property<int>("RoleID")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RoleID");

                    b.ToTable("Accounts", (string)null);
                });

            modelBuilder.Entity("IceCreamBE.Models.Bill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountID")
                        .HasColumnType("int");

                    b.Property<int>("BillDetailID")
                        .HasColumnType("int");

                    b.Property<DateTime>("OrderTime")
                        .HasColumnType("datetime2");

                    b.Property<double>("Status")
                        .HasColumnType("float");

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.Property<int?>("VoucherID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountID");

                    b.HasIndex("VoucherID");

                    b.ToTable("Bill", (string)null);
                });

            modelBuilder.Entity("IceCreamBE.Models.BillDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BillID")
                        .HasColumnType("int");

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
                });

            modelBuilder.Entity("IceCreamBE.Models.Feedback", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountID")
                        .HasColumnType("int");

                    b.Property<string>("FeedBackProduct")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AccountID");

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
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Discount")
                        .HasColumnType("int");

                    b.Property<string>("Img")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int>("Total")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BrandID");

                    b.ToTable("Products", (string)null);
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

                    b.Property<double>("Status")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Recipe", (string)null);
                });

            modelBuilder.Entity("IceCreamBE.Models.Roles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);
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

                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("IceCreamBE.Models.Accounts", b =>
                {
                    b.HasOne("IceCreamBE.Models.Roles", "Role")
                        .WithMany("Accounts")
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

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

            modelBuilder.Entity("IceCreamBE.Models.Feedback", b =>
                {
                    b.HasOne("IceCreamBE.Models.Accounts", "Account")
                        .WithMany("Feedback")
                        .HasForeignKey("AccountID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
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

                    b.Navigation("Feedback");

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
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("IceCreamBE.Models.Vouchers", b =>
                {
                    b.Navigation("Bill");
                });
#pragma warning restore 612, 618
        }
    }
}