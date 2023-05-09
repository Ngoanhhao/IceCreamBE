using IceCreamBE.Data;
using IceCreamBE.Models;
using IceCreamBE.Repository.Irepository;
using Microsoft.EntityFrameworkCore;

namespace IceCreamBE.Repository
{
    public class RepositoryBrand : Repository<Brands>, IRepositoryBrand
    {
        private IceCreamDbcontext dbcontext;
        public RepositoryBrand(IceCreamDbcontext _dbcontext) : base(_dbcontext)
        {
            dbcontext = _dbcontext;
        }

        public async Task UpdateAsync(Brands entity)
        {
            var result = await dbcontext.Brands.FirstOrDefaultAsync(e => e.Id == entity.Id);
            result.BrandName = entity.BrandName;
            await dbcontext.SaveChangesAsync();
        }
    }
}
