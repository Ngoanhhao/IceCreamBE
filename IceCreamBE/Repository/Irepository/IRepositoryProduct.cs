using IceCreamBE.Data;
using IceCreamBE.Models;

namespace IceCreamBE.Repository.Irepository
{
    public interface IRepositoryProduct : IRepository<Products>
    {
        Task UpdateAsync(Products entity);
    }
}
