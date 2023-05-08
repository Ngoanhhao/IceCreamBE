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

            result.ExtensionDate = entity.ExtensionDate;
            result.ExpirationDate = entity.ExpirationDate;
            result.Avatar = entity.Avatar;
            result.PhoneNumber = entity.PhoneNumber;
            result.Email = entity.Email;
            result.FullName = entity.FullName;
            result.RoleID = entity.RoleID;
            await dbcontext.SaveChangesAsync();
        }
    }
}
