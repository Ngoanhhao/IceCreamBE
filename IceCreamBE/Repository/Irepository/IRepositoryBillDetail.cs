using IceCreamBE.Models;

namespace IceCreamBE.Repository.Irepository
{
    public interface IRepositoryBillDetail : IRepository<BillDetail>
    {
        Task UpdateAsync(BillDetail entity);
    }
}
