using IceCreamBE.Data;
using IceCreamBE.DTO;
using IceCreamBE.Models;

namespace IceCreamBE.Repository.Irepository
{
    public interface IRepositoryFeedback : IRepository<Feedback>
    {
        Task UpdateAsync(Feedback entity);
    }
}
