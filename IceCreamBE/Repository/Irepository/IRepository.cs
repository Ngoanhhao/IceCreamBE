using IceCreamBE.DTO;
using IceCreamBE.Models;
using System.Linq.Expressions;

namespace IceCreamBE.Repository.Irepository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>?> query = null);
        Task<T> GetAsync(Expression<Func<T, bool>> query);
        Task CreateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
