using IceCreamBE.DTO;
using IceCreamBE.Models;

namespace IceCreamBE.Repository.Irepository
{
    public interface IRepositoryAccounts : IRepository<Accounts>
    {
        Task UpdateAsync(AccountDTO entity);
    }
}
