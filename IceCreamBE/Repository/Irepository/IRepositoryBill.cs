using IceCreamBE.Models;

namespace IceCreamBE.Repository.Irepository
{
    public interface IRepositoryBill : IRepository<Bill>
    {
        Task UpdateAsync(Bill entity);
        Task UpdateVoucherAsync(Bill entity);
        Task UpdateDiscountAsync(Bill entity, int discount);
        Task UpdateStatusAsync(Bill entity, string? oldStatus);
    }
}
