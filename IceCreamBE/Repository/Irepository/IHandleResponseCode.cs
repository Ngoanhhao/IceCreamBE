using IceCreamBE.DTO;
using IceCreamBE.Models;

namespace IceCreamBE.Repository.Irepository
{
    public interface IHandleResponseCode : IRepository<ResponseCode>
    {
        Task UpdateAsync(ResponseCode entity);
    }
}
