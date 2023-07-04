using IceCreamBE.Data;
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

        public async Task UpdateDiscountAsync(Bill entity, int discount)
        {
            var result = await dbcontext.Bill.FirstOrDefaultAsync(e => e.AccountID == entity.AccountID && e.Status == "ORDERING");
            var total = result.Total;
            result.Discount = discount;
            result.Total = total * (double)(100 - discount) / 100;
            await dbcontext.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(Bill entity, string? oldStatus)
        {
            if (oldStatus != null)
            {
                var result = await dbcontext.Bill.FirstOrDefaultAsync(e => e.AccountID == entity.AccountID && e.Status == oldStatus);
                result.Status = entity.Status;
                result.OrderTime = entity.OrderTime == DateTime.Parse("0001-01-01T00:00:00") ? result.OrderTime : entity.OrderTime;
                await dbcontext.SaveChangesAsync();
            }
            else
            {
                var result = await dbcontext.Bill.FirstOrDefaultAsync(e => e.AccountID == entity.AccountID);
                result.Status = entity.Status;
                result.OrderTime = entity.OrderTime == DateTime.Parse("0001-01-01T00:00:00") ? result.OrderTime : entity.OrderTime;
                await dbcontext.SaveChangesAsync();
            }
        }

        public async Task UpdateVoucherAsync(Bill entity)
        {
            var result = await dbcontext.Bill.FirstOrDefaultAsync(e => e.AccountID == entity.AccountID && e.Status == "ORDERING");
            var voucher = await dbcontext.Vouchers.FirstOrDefaultAsync(e => e.Id == entity.VoucherID);
            if (result != null)
            {
                result.VoucherID = entity.VoucherID;
                result.Total = result.Total * (100 - voucher.Discount) / 100;
                await dbcontext.SaveChangesAsync();
            }
        }
    }
}
