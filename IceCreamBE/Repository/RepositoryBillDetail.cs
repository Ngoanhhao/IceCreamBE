using IceCreamBE.Data;
using IceCreamBE.Models;
using IceCreamBE.Repository.Irepository;
using Microsoft.EntityFrameworkCore;

namespace IceCreamBE.Repository
{
    public class RepositoryBillDetail : Repository<BillDetail>, IRepositoryBillDetail
    {
        private IceCreamDbcontext dbcontext;
        public RepositoryBillDetail(IceCreamDbcontext _dbcontext) : base(_dbcontext)
        {
            dbcontext = _dbcontext;
        }

        public async Task UpdateAsync(BillDetail entity)
        {
            var result = await dbcontext.BillDetail.FirstOrDefaultAsync(e => e.Id == entity.Id);
            result.Total = entity.Total;
            result.Price = entity.Price;
            result.Quantity = entity.Quantity;
            await dbcontext.SaveChangesAsync();
        }
    }
}
