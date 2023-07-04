using IceCreamBE.Data;
using IceCreamBE.DTO;
using IceCreamBE.Models;
using IceCreamBE.Repository.Irepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IceCreamBE.Repository
{
    public class RepositoryAccountDetail : Repository<AccountDetail>, IRepositoryAccountDetail
    {
        private IceCreamDbcontext dbcontext;

        public RepositoryAccountDetail(IceCreamDbcontext _dbcontext) : base(_dbcontext)
        {
            dbcontext = _dbcontext;
        }
        public async Task UpdateAsync(AccountDetailDTO entity)
        {
            var result = await dbcontext.AccountDetail.FirstOrDefaultAsync(e => e.Id == entity.Id);

            result.Avatar = entity.Avatar == null ? result.Avatar : entity.Avatar;
            result.PhoneNumber = entity.Phone_number;
            result.FullName = entity.Full_name;
            result.Address = entity.Address;
            await dbcontext.SaveChangesAsync();
        }

        public async Task UpdatePremium(int userID, int month)
        {
            var result = await dbcontext.AccountDetail.FirstOrDefaultAsync(e => e.Id == userID);
            result.RoleID = 2;
            result.ExtensionDate = DateTime.Now;
            result.ExpirationDate = DateTime.Now.AddMonths(month);
            await dbcontext.SaveChangesAsync();
        }

        public async Task UpdateRole(int userID, int roleID)
        {
            var result = await dbcontext.AccountDetail.FirstOrDefaultAsync(e => e.Id == userID);
            result.RoleID = roleID;
            await dbcontext.SaveChangesAsync();
        }
    }
}
