using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IceCreamBE.Data;
using IceCreamBE.Models;
using IceCreamBE.Repository.Irepository;
using IceCreamBE.DTO.PageList;
using IceCreamBE.DTO;

namespace IceCreamBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private readonly IRepositoryBill _IRepositoryBill;
        private readonly IRepositoryProduct _IRepositoryProduct;
        private readonly IRepositoryAccounts _IRepositoryAccounts;
        private readonly IRepositoryVourcher _IRepositoryVourcher;
        private readonly IRepositoryBillDetail _IRepositoryBillDetail;
        private readonly IRepositoryAccountDetail _IRepositoryAccountDetail;

        public BillsController(
            IRepositoryBill iRepositoryBill,
            IRepositoryProduct iRepositoryProduct,
            IRepositoryAccounts iRepositoryAccounts,
            IRepositoryVourcher iRepositoryVourcher,
            IRepositoryBillDetail iRepositoryBillDetail,
            IRepositoryAccountDetail iRepositoryAccountDetail
            )
        {
            _IRepositoryBill = iRepositoryBill;
            _IRepositoryProduct = iRepositoryProduct;
            _IRepositoryAccounts = iRepositoryAccounts;
            _IRepositoryVourcher = iRepositoryVourcher;
            _IRepositoryBillDetail = iRepositoryBillDetail;
            _IRepositoryAccountDetail = iRepositoryAccountDetail;
        }

        // GET: api/Bills/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BillInDTO>>> GetBill([FromQuery] PaginationFilter<BillInDTO>? filter)
        {
            var bill = await _IRepositoryBill.GetAllAsync();
            var billDetail = await _IRepositoryBillDetail.GetAllAsync();
            //var product = await _IRepositoryProduct.GetAllAsync();
            var accountDetail = await _IRepositoryAccountDetail.GetAllAsync();
            var voucher = await _IRepositoryVourcher.GetAllAsync();
            var account = await _IRepositoryAccounts.GetAllAsync();
            var result = new List<BillOutDTO>();

            var billDT = bill
                    .Join(accountDetail,
                        e => e.AccountID,
                        q => q.Id,
                        (e, q) => new { bill = e, accountDetail = q })
                    .Select(e => new BillOutDTO
                    {
                        Id = e.bill.Id,
                        full_name = e.accountDetail.FullName,
                        order_Time = e.bill.OrderTime,
                        status = e.bill.Status,
                        total = e.bill.Total,
                        voucher = e.bill.VoucherID.ToString(),
                    }).ToList();


            foreach (var e in billDT)
            {
                var billdetail = await _IRepositoryBillDetail.GetAllAsync(q => q.BillID == e.Id);
                var product = await _IRepositoryProduct.GetAllAsync();

                var billDetailItem = billdetail
                    .Join(product,
                        e => e.ProductID,
                        q => q.Id,
                        (e, q) => new { billdetail = e, product = q })
                    .Select(e => new BillDetailOutDTO
                    {
                        Id = e.billdetail.Id,
                        billID = e.billdetail.BillID,
                        product_name = e.product.Name,
                        quantity = e.billdetail.Quantity,
                        total = e.billdetail.Total
                    }).ToList();

                //List<BillDetailOutDTO> billDetailOut = new List<BillDetailOutDTO>();
                //billdetail.ForEach(e =>
                //{
                //    billDetailOut.Add(new BillDetailOutDTO
                //    {
                //        Id = e.Id,
                //        billID = e.BillID,
                //        product_name = e.,

                //    });
                //});

                var voucherItem = (await _IRepositoryVourcher.GetAsync(v => v.Id == int.Parse(e.voucher.Equals("") ? "0" : e.voucher)));
                result.Add(new BillOutDTO
                {
                    Id = e.Id,
                    full_name = e.full_name,
                    order_Time = e.order_Time,
                    status = e.status,
                    total = e.total,
                    voucher = voucherItem == null ? null : voucherItem.Voucher,
                    //bill_detail = billDetailItem
                });
            }

            return Ok(result);

            //var result = bill
            //    .Join(billDetail,
            //        b => b.BillDetailID,
            //        dt => dt.Id,
            //        (b, dt) => new { bill = b, billDetail = dt })
            //.Join(product,
            //    e => e.billDetail.ProductID,
            //    p => p.Id,
            //    (e, p) => new { bill = e.bill, billDetail = e.billDetail, product = p })
            //.Join(account,
            //    e => e.bill.AccountID,
            //    q => q.Id,
            //    (e, q) => new { bill = e.bill, billDetail = e.billDetail, product = e.product, account = q })
            //.Join(accountDetail,
            //    e => e.account.Id,
            //    q => q.Id,
            //    (e, q) => new{ bill = e.bill, billDetail = e.billDetail, product = e.product, account = e.account, accountDetail = q})
            //    .Select(e => new BillOutDTO
            //    {
            //        Id = e.bill.Id,
            //        full_name = e.bill.AccountID.ToString(),
            //        bill_detailID = new billde,
            //        order_Time = e.bill.OrderTime,
            //        status = e.bill.Status,
            //        voucher = e.bill.VoucherID.ToString()
            //    }).ToList();

            //var pageFilter = new PaginationFilter<BillOutDTO>(filter.PageNumber, filter.PageSize);
            //var pagedData = pageFilter.GetPageList(result);

            //return Ok(new PagedResponse<List<BillOutDTO>>
            //{
            //    Data = pagedData,
            //    Succeeded = pagedData == null ? false : true,
            //    Pagination = new PagedResponseDetail<List<BillOutDTO>>
            //    {
            //        current_page = pagedData == null ? 0 : pageFilter.PageNumber,
            //        Page_pize = pagedData == null ? 0 : pageFilter.PageSize,
            //        total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
            //        total_records = result.Count
            //    }
            //});
        }


        // GET: api/Bills/product name
        //[HttpGet("{query}")]
        //public async Task<ActionResult<IEnumerable<Bill>>> GetBill(string query, [FromQuery] PaginationFilter<BillDetailOutDTO>? filter)
        //{
        //    var bill = await _IRepositoryBill.GetAllAsync();
        //    var billDetail = await _IRepositoryBillDetail.GetAllAsync();
        //    var product = await _IRepositoryProduct.GetAllAsync();
        //    var result = bill
        //        .Join(billDetail,
        //            b => b.BillDetailID,
        //            dt => dt.Id,
        //            (b, dt) => new { bill = b, billDetail = dt })
        //        .Join(product,
        //        e => e.billDetail.ProductID,
        //        p => p.Id,
        //        (e, p) => new { bill = e.bill, billDetail = e.billDetail, product = p })
        //        .Where(e => e.product.Name.Contains(query))
        //        .Select(e => new BillDetailOutDTO
        //        {
        //            Id = e.bill.Id,
        //            billID = e.bill.Id,
        //            product_name = e.product.Name,
        //            quantity = e.billDetail.Quantity,
        //            total = e.billDetail.Total,
        //        }).ToList();

        //    var pageFilter = new PaginationFilter<BillDetailOutDTO>(filter.PageNumber, filter.PageSize);
        //    var pagedData = pageFilter.GetPageList(result);

        //    return Ok(new PagedResponse<List<BillDetailOutDTO>>
        //    {
        //        Data = pagedData,
        //        Succeeded = pagedData == null ? false : true,
        //        Pagination = new PagedResponseDetail<List<BillDetailOutDTO>>
        //        {
        //            current_page = pagedData == null ? 0 : pageFilter.PageNumber,
        //            Page_pize = pagedData == null ? 0 : pageFilter.PageSize,
        //            total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
        //            total_records = result.Count
        //        }
        //    });
        //}

        //// GET: api/Bills/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Bill>> GetBill(int id)
        //{
        //    if (_context.Bill == null)
        //    {
        //        return NotFound();
        //    }
        //    var bill = await _context.Bill.FindAsync(id);

        //    if (bill == null)
        //    {
        //        return NotFound();
        //    }

        //    return bill;
        //}

        //// PUT: api/Bills/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutBill(int id, Bill bill)
        //{
        //    if (id != bill.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(bill).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!BillExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Bills
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Bill>> PostBill(Bill bill)
        //{
        //    if (_context.Bill == null)
        //    {
        //        return Problem("Entity set 'IceCreamDbcontext.Bill'  is null.");
        //    }
        //    _context.Bill.Add(bill);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetBill", new { id = bill.Id }, bill);
        //}

        //// DELETE: api/Bills/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteBill(int id)
        //{
        //    if (_context.Bill == null)
        //    {
        //        return NotFound();
        //    }
        //    var bill = await _context.Bill.FindAsync(id);
        //    if (bill == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Bill.Remove(bill);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool BillExists(int id)
        //{
        //    return (_context.Bill?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
