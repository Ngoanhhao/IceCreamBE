using IceCreamBE.Data;
using IceCreamBE.Models;
using IceCreamBE.Repository.Irepository;

namespace IceCreamBE.Repository
{
    public class RepositoryBill : Repository<Bill>, IRepositoryBill
    {
        private IceCreamDbcontext dbcontext;
        public RepositoryBill(IceCreamDbcontext _dbcontext) : base(_dbcontext)
        {
            dbcontext = _dbcontext;
        }

        public Task UpdateAsync(Bill entity)
        {
            throw new NotImplementedException();
        }
    }
}
