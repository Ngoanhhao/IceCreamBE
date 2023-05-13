using IceCreamBE.DTO;
using IceCreamBE.Models;
using System.Linq.Expressions;

namespace IceCreamBE.Repository.Irepository
{
    public interface IRepositoryRoles : IRepository<Roles>
    {
        Task UpdateAsync(Roles entity);
    }
}
