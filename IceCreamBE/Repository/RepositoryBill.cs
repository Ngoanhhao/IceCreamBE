﻿using IceCreamBE.Data;
using IceCreamBE.Models;
using IceCreamBE.Repository.Irepository;
using Microsoft.EntityFrameworkCore;

namespace IceCreamBE.Repository
{
    public class RepositoryBill : Repository<Bill>, IRepositoryBill
    {
        private IceCreamDbcontext dbcontext;
        public RepositoryBill(IceCreamDbcontext _dbcontext) : base(_dbcontext)
        {
            dbcontext = _dbcontext;
        }

        public async Task UpdateAsync(Bill entity)
        {
            var result = await dbcontext.Bill.FirstOrDefaultAsync(e => e.Id == entity.Id);
            result.Status = entity.Status;
            result.VoucherID = entity.VoucherID;
            result.SubTotal = entity.SubTotal;
            result.Total = entity.Total;
            await dbcontext.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(Bill entity)
        {
            var result = await dbcontext.Bill.FirstOrDefaultAsync(e => e.AccountID == entity.AccountID);
            result.Status = entity.Status;
            await dbcontext.SaveChangesAsync();
        }

        public async Task UpdateVoucherAsync(Bill entity)
        {
            var result = await dbcontext.Bill.FirstOrDefaultAsync(e => e.AccountID == entity.AccountID && e.Status == "ORDERING");
            if (result != null)
            {
                result.VoucherID = entity.VoucherID;
                await dbcontext.SaveChangesAsync();
            }
        }
    }
}
