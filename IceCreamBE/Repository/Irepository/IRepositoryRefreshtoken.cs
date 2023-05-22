using IceCreamBE.Models;

namespace IceCreamBE.Repository.Irepository
{
    public interface IRepositoryRefreshtoken : IRepository<RefreshToken>
    {
        Task UpdateAsync(RefreshToken entity);
    }
}
