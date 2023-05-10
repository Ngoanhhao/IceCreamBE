using IceCreamBE.Data;
using IceCreamBE.Models;
using IceCreamBE.Repository.Irepository;
using Microsoft.EntityFrameworkCore;

namespace IceCreamBE.Repository
{
    public class RepositoryRecipe : Repository<Recipe>, IRepositoryRecipe
    {
        private IceCreamDbcontext dbcontext;
        public RepositoryRecipe(IceCreamDbcontext _dbcontext) : base(_dbcontext)
        {
            dbcontext = _dbcontext;
        }

        public async Task UpdateAsync(Recipe entity)
        {
            var result = await dbcontext.Recipe.FirstOrDefaultAsync(e => e.Id == entity.Id);    
            result.Description = entity.Description;
            result.Status = entity.Status;
            await dbcontext.SaveChangesAsync();
        }
    }
}
