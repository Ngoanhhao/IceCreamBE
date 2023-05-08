using IceCreamBE.Data;
using IceCreamBE.Models;
using IceCreamBE.Repository.Irepository;
using Microsoft.EntityFrameworkCore;

namespace IceCreamBE.Repository
{
    public class RepositoryStorage : Repository<Storage>, IRepositoryStorage
    {
        private IceCreamDbcontext dbcontext;
        public RepositoryStorage(IceCreamDbcontext _dbcontext) : base(_dbcontext)
        {
            dbcontext = _dbcontext;
        }

        public async Task UpdateAsync(int id, int quantity)
        {
            var value = await dbcontext.storage.FirstOrDefaultAsync(e => e.ProductID == id);
            value.Quantity = quantity;
            await dbcontext.SaveChangesAsync();
        }
    }
}
