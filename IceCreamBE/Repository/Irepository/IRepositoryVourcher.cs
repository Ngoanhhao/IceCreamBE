using IceCreamBE.Models;

namespace IceCreamBE.Repository.Irepository
{
    public interface IRepositoryVourcher : IRepository<Vouchers>
    {
        Task UpdateAsync(Products entity);
    }
}
