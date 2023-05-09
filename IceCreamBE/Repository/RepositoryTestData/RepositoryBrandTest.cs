using IceCreamBE.Data;
using IceCreamBE.Models;
using IceCreamBE.Repository.Irepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IceCreamBE.Repository.RepositoryTest
{
    public class RepositoryBrandTest : IRepositoryBrand
    {
        private List<Brands> brandList = new List<Brands>();
        private IceCreamDbcontext dbcontext;
        public RepositoryBrandTest(IceCreamDbcontext dbcontext)
        {
            this.dbcontext = dbcontext;

            for (int i = 1; i <= 24; i++)
            {
                brandList.Add(new Brands { Id = i, BrandName = Guid.NewGuid().ToString() });
            }

        }

        public async Task CreateAsync(Brands entity)
        {
            await dbcontext.Brands.AddAsync(entity);
            await dbcontext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Brands entity)
        {
            dbcontext.Brands.Remove(entity);
            await dbcontext.SaveChangesAsync();
        }

        public async Task<List<Brands>> GetAllAsync(Expression<Func<Brands, bool>?> query = null)
        {
            //IQueryable<Brands> cm = dbcontext.Brands;
            //if (query != null)
            //{
            //    cm = cm.Where(query);
            //}
            return brandList;
        }

        public async Task<Brands> GetAsync(Expression<Func<Brands, bool>> query)
        {
            IQueryable<Brands> cm = dbcontext.Brands;
            if (query != null)
            {
                cm = cm.Where(query);
            }
            return await cm.FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Brands entity)
        {
            var result = await dbcontext.Brands.FirstOrDefaultAsync(e => e.Id == entity.Id);
            result.BrandName = entity.BrandName;
            await dbcontext.SaveChangesAsync();
        }
    }
}
