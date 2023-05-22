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
using Microsoft.AspNetCore.Authorization;

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
        [HttpGet, Authorize(Roles = "Member, Admin")]
        public async Task<ActionResult<IEnumerable<BillInDTO>>> GetBill([FromQuery] PaginationFilter<BillInDTO>? filter)
        {
            var bill = await _IRepositoryBill.GetAllAsync();
            var accountDetail = await _IRepositoryAccountDetail.GetAllAsync();

            // Bill
            var result = bill
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
                        sub_total = e.bill.SubTotal,
                        email = e.accountDetail.Email,
                        phone_number = e.accountDetail.PhoneNumber,
                    }).ToList();


            foreach (var e in result)
            {
                var billdetail = await _IRepositoryBillDetail.GetAllAsync(q => q.BillID == e.Id);
                var product = await _IRepositoryProduct.GetAllAsync();

                // get bill detail
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
                        total = e.billdetail.Total,
                        price = e.billdetail.Price,
                    })
                    .ToList();

                //get voucher from voucherID
                var voucherItem = (await _IRepositoryVourcher.GetAsync(v => v.Id == int.Parse(e.voucher.Equals("") ? "0" : e.voucher)));

                e.voucher = voucherItem == null ? null : voucherItem.Voucher;
                e.products = billDetailItem;
            }

            var pageFilter = new PaginationFilter<BillOutDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(result);

            return Ok(new PagedResponse<List<BillOutDTO>>
            {
                Data = pagedData,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<BillOutDTO>>
                {
                    current_page = pagedData == null ? 0 : pageFilter.PageNumber,
                    Page_pize = pagedData == null ? 0 : pageFilter.PageSize,
                    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                    total_records = result.Count
                }
            });
        }


        //GET: api/Bills/name, email, phone number
        [HttpGet("{query}")]
        public async Task<ActionResult<IEnumerable<BillOutDTO>>> GetBill(string query, [FromQuery] PaginationFilter<BillOutDTO>? filter)
        {
            var bill = await _IRepositoryBill.GetAllAsync();
            var accountDetail = await _IRepositoryAccountDetail.GetAllAsync();

            // Bill
            var result = bill
                    .Join(accountDetail,
                        e => e.AccountID,
                        q => q.Id,
                        (e, q) => new { bill = e, accountDetail = q })
                    .Where(e => e.accountDetail.FullName.Contains(query) || e.accountDetail.Email.Contains(query) || e.accountDetail.PhoneNumber.Contains(query))
                    .Select(e => new BillOutDTO
                    {
                        Id = e.bill.Id,
                        full_name = e.accountDetail.FullName,
                        order_Time = e.bill.OrderTime,
                        status = e.bill.Status,
                        total = e.bill.Total,
                        voucher = e.bill.VoucherID.ToString(),
                        sub_total = e.bill.SubTotal,
                        email = e.accountDetail.Email,
                        phone_number = e.accountDetail.PhoneNumber,
                    })
                    .ToList();

            if (result.Count == 0)
            {
                return NotFound(new Response<BillOutDTO> { Message = "not found", Succeeded = false });
            }

            foreach (var e in result)
            {
                // get bill detail
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
                        total = e.billdetail.Total,
                        price = e.billdetail.Price,
                    })
                    .ToList();

                //get voucher from voucherID
                var voucherItem = (await _IRepositoryVourcher.GetAsync(v => v.Id == int.Parse(e.voucher.Equals("") ? "0" : e.voucher)));

                e.voucher = voucherItem == null ? null : voucherItem.Voucher;
                e.products = billDetailItem;
            }

            var pageFilter = new PaginationFilter<BillOutDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(result);

            return Ok(new PagedResponse<List<BillOutDTO>>
            {
                Data = pagedData,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<BillOutDTO>>
                {
                    current_page = pagedData == null ? 0 : pageFilter.PageNumber,
                    Page_pize = pagedData == null ? 0 : pageFilter.PageSize,
                    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                    total_records = result.Count
                }
            });
        }

        //GET: api/Bills/id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<IEnumerable<BillOutDTO>>> GetBill(int id, [FromQuery] PaginationFilter<BillOutDTO>? filter)
        {
            var bill = await _IRepositoryBill.GetAllAsync();
            var accountDetail = await _IRepositoryAccountDetail.GetAllAsync();

            // Bill
            var result = bill
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
                        sub_total = e.bill.SubTotal,
                        email = e.accountDetail.Email,
                        phone_number = e.accountDetail.PhoneNumber,
                    }).FirstOrDefault(e => e.Id == id);

            if (result == null)
            {
                return NotFound(new Response<BillOutDTO> { Message = "not found", Succeeded = false });
            }

            // get bill detail
            var billdetail = await _IRepositoryBillDetail.GetAllAsync(q => q.BillID == result.Id);
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
                    total = e.billdetail.Total,
                    price = e.billdetail.Price,
                })
                .ToList();

            //get voucher from voucherID
            var voucherItem = (await _IRepositoryVourcher.GetAsync(v => v.Id == int.Parse(result.voucher.Equals("") ? "0" : result.voucher)));

            result.voucher = voucherItem == null ? null : voucherItem.Voucher;
            result.products = billDetailItem;

            return Ok(new Response<BillOutDTO> { Data = result, Succeeded = true });
        }

        // PUT: api/Bills/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBill(int id, BillInDTO bill)
        {
            if (id != bill.Id)
            {
                return BadRequest(new Response<BillOutDTO> { Message = "value incorrect", Succeeded = false });
            }

            var result = await _IRepositoryBill.GetAsync(e => e.Id == id);

            if (result == null)
            {
                return NotFound(new Response<BillOutDTO> { Message = "not found", Succeeded = false });
            }

            await _IRepositoryBill.UpdateAsync(new Bill
            {
                Id = bill.Id,
                Status = bill.status,
                VoucherID = bill.voucherID,
            });

            return NoContent();
        }

        // POST: api/Bills
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BillOutDTO>> PostBill(BillInDTO bill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<BillOutDTO> { Message = "value incorrect", Succeeded = false });
            }

            await _IRepositoryBill.CreateAsync(new Bill
            {
                AccountID = bill.accountID,
                VoucherID = bill.voucherID,
                Status = false,
            });

            return CreatedAtAction("GetBill", new { id = bill.Id }, bill);
        }

        //// DELETE: api/Bills/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBill(int id)
        {
            var result = await _IRepositoryBill.GetAsync(e => e.Id == id);

            if (result == null)
            {
                return NotFound(new Response<BillOutDTO> { Message = "not found", Succeeded = false });
            }

            await _IRepositoryBill.DeleteAsync(result);
            var billList = await _IRepositoryBillDetail.GetAllAsync(e => e.BillID == result.Id);
            billList.ForEach(async e => await _IRepositoryBillDetail.DeleteAsync(e));


            return NoContent();
        }
    }
}
