using IceCreamBE.DTO;
using IceCreamBE.Models;

namespace IceCreamBE.Repository.Irepository
{
    public interface IRepositoryAccountDetail : IRepository<AccountDetail>
    {
        Task UpdateAsync(AccountDetailDTO entity);
    }
}
