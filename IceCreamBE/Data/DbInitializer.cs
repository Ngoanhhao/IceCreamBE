using IceCreamBE.Models;
using Microsoft.EntityFrameworkCore;

namespace IceCreamBE.Data
{
    public class DbInitializer
    {
        private readonly ModelBuilder modelBuilder;

        public DbInitializer(ModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }

        public void Seed()
        {
            modelBuilder.Entity<Roles>().HasData(
                new Roles { Id = 1, Role = "Admin" },
                new Roles { Id = 2, Role = "Member" },
                new Roles { Id = 3, Role = "Guest" }
            );

            modelBuilder.Entity<Accounts>().HasData(
                new Accounts
                {
                    Id = 1,
                    Username = "Admin",
                    Password = "Admin"
                }
            );

            modelBuilder.Entity<AccountDetail>().HasData(
                new AccountDetail
                {
                    Id = 1,
                    Email = "ngoanhhao24@gmail.com",
                    FullName = "Ngô Anh Hào",
                    PhoneNumber = "1234567890",
                    ExpirationDate = DateTime.Now.AddDays(10),
                    ExtensionDate = DateTime.Now,
                    RoleID = 1,
                }
            );

            modelBuilder.Entity<Brands>().HasData(new Brands
            {
                Id = 1,
                BrandName = "Pepsi"
            });

            modelBuilder.Entity<Products>().HasData(new Products
            {
                Id = 1,
                BrandID = 1,
                Cost = 10000,
                Name = "Pepsi",
                Price = 15000,
                Status = true,
                Total = 15000,
            });

            modelBuilder.Entity<Bill>().HasData(new Bill
            {
                Id = 1,
                AccountID = 1,
                OrderTime = DateTime.Now,
                Status = true,
                SubTotal = 30000,
                Total = 30000,
            });

            modelBuilder.Entity<BillDetail>().HasData(new BillDetail
            {
                Id = 1,
                BillID = 1,
                ProductID = 1,
                Price = 15000,
                Quantity = 2,
                Total = 30000,
            });
        }
    }
}
