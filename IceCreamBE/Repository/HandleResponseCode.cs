using IceCreamBE.Data;
using IceCreamBE.Models;
using IceCreamBE.Repository.Irepository;
using Microsoft.EntityFrameworkCore;

namespace IceCreamBE.Repository
{
    public class HandleResponseCode : Repository<ResponseCode>, IHandleResponseCode
    {
        private IceCreamDbcontext dbcontext;

        public HandleResponseCode(IceCreamDbcontext _dbcontext) : base(_dbcontext)
        {
            dbcontext = _dbcontext;
        }

        public async Task UpdateAsync(ResponseCode entity)
        {
            var result = await dbcontext.ResponseCode.FirstOrDefaultAsync(e => e.Code == entity.Code && e.Email == entity.Email);
            if (!result.Status)
            {
                result.Status = true;
                await dbcontext.SaveChangesAsync();
            }
        }
    }
}
