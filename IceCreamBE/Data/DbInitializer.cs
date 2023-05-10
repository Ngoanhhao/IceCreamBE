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
        }
    }
}
