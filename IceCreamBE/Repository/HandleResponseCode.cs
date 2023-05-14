using IceCreamBE.Data;
using IceCreamBE.Models;

namespace IceCreamBE.Repository
{
    public class HandleResponseCode : Repository<ResponseCode>
    {
        public HandleResponseCode(IceCreamDbcontext _dbcontext) : base(_dbcontext)
        {
        }
    }
}
