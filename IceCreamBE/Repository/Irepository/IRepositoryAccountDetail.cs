﻿using IceCreamBE.DTO;
using IceCreamBE.Models;

namespace IceCreamBE.Repository.Irepository
{
    public interface IRepositoryAccountDetail : IRepository<AccountDetail>
    {
        Task UpdateAsync(AccountDetailDTO entity);
        Task UpdatePremium(int userID, int month);
        Task UpdateRole(int userID, int roleID);
    }
}
