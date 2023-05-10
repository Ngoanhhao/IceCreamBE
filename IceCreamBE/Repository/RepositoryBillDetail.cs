using IceCreamBE.Data;
using IceCreamBE.Models;
using IceCreamBE.Repository.Irepository;

namespace IceCreamBE.Repository
{
    public class RepositoryBillDetail : Repository<BillDetail>, IRepositoryBillDetail
    {
        private IceCreamDbcontext dbcontext;
        public RepositoryBillDetail(IceCreamDbcontext _dbcontext) : base(_dbcontext)
        {
            dbcontext = _dbcontext;
        }

        public Task UpdateAsync(BillDetail entity)
        {
            throw new NotImplementedException();
        }
    }
}
