using IceCreamBE.Data;
using IceCreamBE.DTO;
using IceCreamBE.Models;
using IceCreamBE.Repository.Irepository;
using Microsoft.EntityFrameworkCore;

namespace IceCreamBE.Repository
{
    public class RepositoryFeedback : Repository<Feedback>, IRepositoryFeedback
    {
        private IceCreamDbcontext dbcontext;
        public RepositoryFeedback(IceCreamDbcontext _dbcontext) : base(_dbcontext)
        {
            dbcontext = _dbcontext;
        }

        public async Task UpdateAsync(Feedback entity)
        {
            var result = await dbcontext.Feedback.FirstOrDefaultAsync(e => e.Id == entity.Id);
            result.status = result.status ? false : true;
            await dbcontext.SaveChangesAsync();
        }
    }
}
