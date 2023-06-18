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

            result.ExtensionDate = entity.Extension_date ?? result.ExtensionDate;
            result.ExpirationDate = entity.Expiration_date ?? result.ExpirationDate;
            result.Avatar = entity.Avatar == null ? result.Avatar : entity.Avatar;
            result.PhoneNumber = entity.Phone_number;
            result.Email = entity.Email;
            result.FullName = entity.Full_name;
            result.Address = entity.Address;
            result.RoleID = entity.RoleID ?? result.RoleID;
            await dbcontext.SaveChangesAsync();
        }
    }
}
