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
    public class RepositoryRoles : Repository<Roles>, IRepositoryRoles
    {
        IceCreamDbcontext dbcontext;

        public RepositoryRoles(IceCreamDbcontext _dbcontext) : base(_dbcontext)
        {
            dbcontext = _dbcontext;
        }

        public async Task UpdateAsync(Roles roles)
        {
            var item = await dbcontext.Roles.FirstOrDefaultAsync(e => e.Id == roles.Id);
            item.Role = roles.Role;
            await dbcontext.SaveChangesAsync();
        }
    }
}
