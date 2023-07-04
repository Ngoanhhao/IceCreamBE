using IceCreamBE.Data;
using IceCreamBE.Models;

namespace IceCreamBE.Repository.Irepository
{
    public interface IRepositoryStorage : IRepository<Storage>
    {
        Task UpdateAsync(int id, int quantity, bool In = true);
    }
}
