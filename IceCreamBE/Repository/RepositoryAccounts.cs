using IceCreamBE.Data;
using IceCreamBE.DTO;
using IceCreamBE.Models;
using IceCreamBE.Repository.Irepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IceCreamBE.Repository
{
    public class RepositoryAccounts : Repository<Accounts>, IRepositoryAccounts
    {
        private IceCreamDbcontext dbcontext;

        public RepositoryAccounts(IceCreamDbcontext _dbcontext) : base(_dbcontext)
        {
            dbcontext = _dbcontext;
        }
        public async Task UpdateAsync(AccountDTO entity)
        {
            var result = await dbcontext.Accounts.FirstOrDefaultAsync(e => e.Id == entity.Id);
            result.Password = entity.Password;
            await dbcontext.SaveChangesAsync();
        }
    }
}
