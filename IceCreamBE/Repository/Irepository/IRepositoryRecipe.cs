using IceCreamBE.Models;

namespace IceCreamBE.Repository.Irepository
{
    public interface IRepositoryRecipe : IRepository<Recipe>
    {
        Task UpdateAsync(Recipe entity);
    }
}
