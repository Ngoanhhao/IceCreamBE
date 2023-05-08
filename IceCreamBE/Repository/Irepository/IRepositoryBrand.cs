using IceCreamBE.Models;

namespace IceCreamBE.Repository.Irepository
{
    public interface IRepositoryBrand : IRepository<Brands>
    {
        Task UpdateAsync(Brands entity);
    }
}
