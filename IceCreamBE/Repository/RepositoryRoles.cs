using IceCreamBE.Data;
using IceCreamBE.DTO;
using IceCreamBE.Models;
using IceCreamBE.Repository.Irepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace IceCreamBE.Repository
{
    public class RepositoryRoles : IRepositoryRoles
    {
        IceCreamDbcontext dbcontext;
        DbSet<Roles> set;
        public RepositoryRoles(IceCreamDbcontext _dbcontext)
        {
            dbcontext = _dbcontext;
            set = dbcontext.Roles;
        }

        public async Task<RolesDTO> CreateAsync(RolesDTO entity)
        {
            dbcontext.Roles.Add(new Roles { Role = entity.Role });
            await dbcontext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(RolesDTO entity)
        {
            var result = await dbcontext.Roles.FirstOrDefaultAsync(e => e.Id == entity.Id);
            dbcontext.Roles.Remove(result);
            await dbcontext.SaveChangesAsync();
        }

        public async Task<IEnumerable<RolesDTO>> GetAllAsync(Expression<Func<Roles, bool>?> query = null)
        {
            List<RolesDTO> value = new List<RolesDTO>();
            IQueryable<Roles> cm = set;
            if (query != null)
            {
                cm = cm.Where(query);
            }
            await cm.ForEachAsync(e => value.Add(new RolesDTO { Id = e.Id, Role = e.Role }));
            return value;
        }

        public async Task<RolesDTO> GetAsync(Expression<Func<Roles, bool>> query)
        {
            IQueryable<Roles> cm = dbcontext.Roles;
            if (query != null)
            {
                cm = cm.Where(query);
            }
            var item = await cm.FirstOrDefaultAsync();
            return new RolesDTO { Id = item.Id, Role = item.Role };
        }

        public async Task UpdateAsync(RolesDTO roles)
        {
            var item = await dbcontext.Roles.FirstOrDefaultAsync(e => e.Id == roles.Id);
            item.Role = roles.Role;
            await dbcontext.SaveChangesAsync();
        }
    }
}
