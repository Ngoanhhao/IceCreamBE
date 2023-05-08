using IceCreamBE.Data;
using IceCreamBE.DTO;
using IceCreamBE.Models;
using IceCreamBE.Repository.Irepository;
using Microsoft.EntityFrameworkCore;

namespace IceCreamBE.Repository
{
    public class RepositoryProducts : Repository<Products>, IRepositoryProduct
    {
        private IceCreamDbcontext dbcontext;
        public RepositoryProducts(IceCreamDbcontext _dbcontext) : base(_dbcontext)
        {
            dbcontext = _dbcontext;
        }

        public Task UpdateAsync(Products entity)
        {
            throw new NotImplementedException();
        }
    }
}
