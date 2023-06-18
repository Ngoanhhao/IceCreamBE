using IceCreamBE.Data;
using IceCreamBE.DTO;
using IceCreamBE.Models;
using IceCreamBE.Repository.Irepository;
using Microsoft.EntityFrameworkCore;

namespace IceCreamBE.Repository
{
    public class RepositoryProducts : Repository<Products>, IRepositoryProduct
    {
        private IceCreamDbcontext dbcontext;
        public RepositoryProducts(IceCreamDbcontext _dbcontext) : base(_dbcontext)
        {
            dbcontext = _dbcontext;
        }

        public async Task UpdateAsync(Products entity)
        {
            var result = await dbcontext.Products.FirstOrDefaultAsync(e => e.Id == entity.Id);
            result.Status = entity.Status;
            result.Description = entity.Description;
            result.Cost = entity.Cost;
            result.Price = entity.Price;
            result.Discount = entity.Discount;
            result.BrandID = entity.BrandID;
            result.Img = entity.Img == null ? result.Img : entity.Img;
            result.Name = entity.Name;
            result.Total = (double)(((100 - entity.Discount) * 0.01) * entity.Price);
            await dbcontext.SaveChangesAsync();
        }
    }
}
