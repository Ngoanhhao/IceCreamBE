using IceCreamBE.Data;
using IceCreamBE.Models;
using IceCreamBE.Repository.Irepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IceCreamBE.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        IceCreamDbcontext dbcontext;
        DbSet<T> set;
        public Repository(IceCreamDbcontext _dbcontext)
        {
            dbcontext = _dbcontext;
            set = _dbcontext.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
            await set.AddAsync(entity);
            await dbcontext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            set.Remove(entity);
            await dbcontext.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>?> query = null)
        {
            IQueryable<T> cm = set;
            if (query != null)
            {
                cm = cm.Where(query);
            }
            return await cm.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> query)
        {
            IQueryable<T> cm = set;
            if (query != null)
            {
                cm = cm.Where(query);
            }
            return await cm.FirstOrDefaultAsync();
        }
    }
}
