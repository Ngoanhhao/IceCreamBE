using IceCreamBE.DTO;
using IceCreamBE.Models;
using System.Linq.Expressions;

namespace IceCreamBE.Repository.Irepository
{
    public interface IRepositoryRoles
    {
        Task<IEnumerable<RolesDTO>> GetAllAsync(Expression<Func<Roles, bool>?> query = null);
        Task<RolesDTO> GetAsync(Expression<Func<Roles, bool>> query);
        Task<RolesDTO> CreateAsync(RolesDTO entity);
        Task UpdateAsync(RolesDTO entity);
        Task DeleteAsync(RolesDTO entity);
    }
}
