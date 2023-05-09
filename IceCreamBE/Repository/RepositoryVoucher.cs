using IceCreamBE.Data;
using IceCreamBE.Models;
using IceCreamBE.Repository.Irepository;
using Microsoft.EntityFrameworkCore;

namespace IceCreamBE.Repository
{
    public class RepositoryVoucher : Repository<Vouchers>, IRepositoryVourcher
    {
        private IceCreamDbcontext dbcontext;
        public RepositoryVoucher(IceCreamDbcontext _dbcontext) : base(_dbcontext)
        {
            dbcontext = _dbcontext;
        }
        public async Task UpdateAsync(Vouchers entity)
        {
            var result = await dbcontext.Vouchers.FirstOrDefaultAsync(e => e.Id == entity.Id);
            result.Status = entity.Status;  
            await dbcontext.SaveChangesAsync();
        }
    }
}
