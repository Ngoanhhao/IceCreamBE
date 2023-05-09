using IceCreamBE.Data;
using IceCreamBE.Models;
using IceCreamBE.Repository.Irepository;

namespace IceCreamBE.Repository
{
    public class RepositoryVoucher : Repository<Vouchers>, IRepositoryVourcher
    {
        private IceCreamDbcontext dbcontext;
        public RepositoryVoucher(IceCreamDbcontext _dbcontext) : base(_dbcontext)
        {
            dbcontext = _dbcontext;
        }
        public Task UpdateAsync(Products entity)
        {
            throw new NotImplementedException();
        }
    }
}
