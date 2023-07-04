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

        public async Task UpdateAsync(int id, int quantity, bool In = true)
        {
            var value = await dbcontext.storage.FirstOrDefaultAsync(e => e.ProductID == id);
            if (In)
            {
                value.Quantity = value.Quantity + quantity;
                value.LastOrder = DateTime.Now;
            }
            else
            {
                value.Quantity = value.Quantity + quantity;
            }
            await dbcontext.SaveChangesAsync();
        }
    }
}
