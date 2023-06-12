using IceCreamBE.Models;

namespace IceCreamBE.Repository.Irepository
{
    public interface IRepositoryBill : IRepository<Bill>
    {
        Task UpdateAsync(Bill entity);
        Task UpdateVoucherAsync(Bill entity);
        Task UpdateStatusAsync(Bill entity);
    }
}
