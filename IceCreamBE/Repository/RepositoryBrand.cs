using IceCreamBE.Data;
using IceCreamBE.Models;
using IceCreamBE.Repository.Irepository;

namespace IceCreamBE.Repository
{
    public class RepositoryBrand : Repository<Brands>, IRepositoryBrand
    {
        private IceCreamDbcontext dbcontext;
        public RepositoryBrand(IceCreamDbcontext _dbcontext) : base(_dbcontext)
        {
            dbcontext = _dbcontext;
        }

        public Task UpdateAsync(Brands entity)
        {
            throw new NotImplementedException();
        }
    }
}
