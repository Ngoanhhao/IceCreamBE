using IceCreamBE.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace IceCreamBE.Data
{
    public class IceCreamDbcontext : DbContext
    {
        public IceCreamDbcontext(DbContextOptions<IceCreamDbcontext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            // table config
            modelBuilder.Entity<Accounts>(entity =>
            {
                entity.ToTable("Accounts");
                entity.HasKey(x => x.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Username).HasMaxLength(255);
                entity.HasOne<AccountDetail>(x => x.AccountDetail)
                    .WithOne(e => e.Accounts)
                    .HasForeignKey<AccountDetail>(e => e.Id);
                entity.HasMany<Feedback>(e => e.Feedback)
                    .WithOne(e => e.Account)
                    .HasForeignKey(e => e.AccountID);
                entity.HasMany<Bill>(e => e.Bill)
                    .WithOne(e => e.Account)
                    .HasForeignKey(e => e.AccountID);
                entity.HasMany<Vouchers>(e => e.vouchers)
                    .WithOne(e => e.Admin)
                    .HasForeignKey(e => e.AdminID);
            });

            modelBuilder.Entity<AccountDetail>(entity =>
            {
                entity.ToTable("AccountDetail");
                entity.HasKey(x => x.Id);
                entity.HasOne<Roles>(e => e.Role)
                    .WithMany(e => e.AccountDetail)
                    .HasForeignKey(e => e.RoleID);
                entity.HasOne<Accounts>(x => x.Accounts)
                    .WithOne(e => e.AccountDetail)
                    .HasForeignKey<AccountDetail>(e => e.Id);
                entity.Property(e => e.PhoneNumber).HasMaxLength(11);
                entity.Property(e => e.Email).HasMaxLength(100);
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.ToTable("Roles");
                entity.Property(e => e.Role).HasMaxLength(50);
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasMany(e => e.AccountDetail)
                    .WithOne(e => e.Role)
                    .HasForeignKey(e => e.RoleID)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasOne<Accounts>(e => e.Account)
                    .WithMany(e => e.Feedback)
                    .HasForeignKey(e => e.AccountID);
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasOne<Storage>(e => e.Storage)
                    .WithOne(e => e.Product)
                    .HasForeignKey<Storage>(e => e.ProductID);
                entity.Property(e => e.Name).HasMaxLength(255);
                entity.HasMany<BillDetail>(e => e.Details)
                    .WithOne(e => e.Product)
                    .HasForeignKey(e => e.ProductID);
                entity.HasMany<Recipe>(e => e.Recipe)
                    .WithOne(e => e.Product)
                    .HasForeignKey(e => e.ProductId);
            });

            modelBuilder.Entity<Storage>(entity =>
            {
                entity.ToTable("Storage");
                entity.HasKey(e => e.ProductID);
                entity.HasOne<Products>(e => e.Product)
                    .WithOne(e => e.Storage)
                    .HasForeignKey<Storage>(e => e.ProductID);
            });

            modelBuilder.Entity<BillDetail>(entity =>
            {
                entity.ToTable("BillDetail");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasOne<Bill>(e => e.Bill)
                    .WithMany(e => e.BillDetail)
                    .HasForeignKey(e => e.BillID);
                entity.HasOne<Products>(e => e.Product)
                    .WithMany(e => e.Details)
                    .HasForeignKey(e => e.ProductID);
            });

            modelBuilder.Entity<Bill>(entity =>
            {
                entity.ToTable("Bill");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasMany<BillDetail>(e => e.BillDetail)
                    .WithOne(e => e.Bill)
                    .HasForeignKey(e => e.BillID);
                entity.HasOne<Accounts>(e => e.Account)
                    .WithMany(e => e.Bill)
                    .HasForeignKey(e => e.AccountID);
                entity.HasOne<Vouchers>(e => e.Vouchers)
                    .WithMany(e => e.Bill)
                    .HasForeignKey(e => e.VoucherID)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.ToTable("Recipe");
                entity.HasKey(e => e.Id);
                entity.HasOne<Products>(e => e.Product)
                    .WithMany(e => e.Recipe)
                    .HasForeignKey(e => e.ProductId);
            });

            modelBuilder.Entity<Vouchers>(entity =>
            {
                entity.ToTable("Voucher");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasOne<Accounts>(e => e.Admin)
                    .WithMany(e => e.vouchers)
                    .HasForeignKey(e => e.AdminID);
                entity.HasMany<Bill>(e => e.Bill)
                    .WithOne(e => e.Vouchers)
                    .HasForeignKey(e => e.VoucherID)
                    .OnDelete(DeleteBehavior.NoAction);
            });


            base.OnModelCreating(modelBuilder);
            new DbInitializer(modelBuilder).Seed();
        }

        public DbSet<AccountDetail> AccountDetail { get; set; }
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Storage> storage { get; set; }
        public DbSet<BillDetail> BillDetail { get; set; }
        public DbSet<Bill> Bill { get; set; }
        public DbSet<Brands> Brands { get; set; }
        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<Vouchers> Vouchers { get; set; }
        public DbSet<Feedback> Feedback { get; set; }

    }
}
