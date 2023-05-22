using IceCreamBE.Data;
using IceCreamBE.Models;
using IceCreamBE.Repository.Irepository;
using Microsoft.EntityFrameworkCore;

namespace IceCreamBE.Repository
{
    public class RepositoryRefreshToken : Repository<RefreshToken>, IRepositoryRefreshtoken
    {
        private IceCreamDbcontext dbcontext;
        public RepositoryRefreshToken(IceCreamDbcontext _dbcontext) : base(_dbcontext)
        {
            dbcontext = _dbcontext;
        }

        public async Task UpdateAsync(RefreshToken entity)
        {
            var item = await dbcontext.RefreshToken.FirstOrDefaultAsync(e => e.id == entity.id);
            item.isUsed = true;
            await dbcontext.SaveChangesAsync();
        }
    }
}
