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
        [HttpGet]
        //[Authorize(Roles = "Member, Admin")]
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
        public async Task<ActionResult<IEnumerable<BillOutDTO>>> GetBills(int id)
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


        //GET: api/Bills/order/id
        [HttpGet("/getOrder/{userid:int}")]
        public async Task<ActionResult<IEnumerable<BillOutDTO>>> GetBill(int userid)
        {
            var bill = await _IRepositoryBill.GetAllAsync();
            var accountDetail = await _IRepositoryAccountDetail.GetAllAsync();

            // Bill
            var result01 = bill
                    .Join(accountDetail,
                        e => e.AccountID,
                        q => q.Id,
                        (e, q) => new { bill = e, accountDetail = q })
                    .FirstOrDefault(e => e.accountDetail.Id == userid && e.bill.Status == "ORDERING");
            if (result01 == null)
            {
                return NotFound(new Response<BillOutDTO> { Message = "not found", Succeeded = false });
            }
            var result = new BillOutDTO
            {
                Id = result01.bill.Id,
                full_name = result01.accountDetail.FullName,
                order_Time = result01.bill.OrderTime,
                status = result01.bill.Status,
                total = result01.bill.Total,
                voucher = result01.bill.VoucherID.ToString(),
                sub_total = result01.bill.SubTotal,
                email = result01.accountDetail.Email,
                phone_number = result01.accountDetail.PhoneNumber,
            };



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
        [HttpPut("{userID}")]
        public async Task<IActionResult> PutBill(int userID, string voucher)
        {
            var getVoucher = await _IRepositoryVourcher.GetAsync(e => e.Voucher == voucher);

            if (getVoucher == null)
            {
                return NotFound(new Response<BillOutDTO> { Message = "voucher invalid", Succeeded = false });
            }
            else if (getVoucher.ExpirationDate < DateTime.Now)
            {
                return NotFound(new Response<BillOutDTO> { Message = "the voucher has expired", Succeeded = false });
            }


            var result = await _IRepositoryBill.GetAsync(e => e.AccountID == userID && e.Status == "ORDERING");

            if (result == null)
            {
                return NotFound(new Response<BillOutDTO> { Message = "account invalid or the bill not available", Succeeded = false });
            }

            await _IRepositoryBill.UpdateVoucherAsync(new Bill
            {
                AccountID = userID,
                VoucherID = getVoucher.Id,
            });

            // update subtotal total bill
            var bill = await _IRepositoryBill.GetAsync(e => e.Status == "ORDERING" && e.AccountID == userID);
            var BillDetails = await _IRepositoryBillDetail.GetAllAsync(e => e.BillID == bill.Id);
            var voucher2 = await _IRepositoryVourcher.GetAsync(e => e.Id == bill.VoucherID);
            double subTotal = 0;
            BillDetails.ForEach(e => subTotal += e.Total);
            var total = voucher != null ? (100 - voucher2.Discount) * 0.01 * subTotal : subTotal;
            await _IRepositoryBill.UpdateAsync(new Bill
            {
                Id = bill.Id,
                VoucherID = bill.VoucherID,
                Status = bill.Status,
                SubTotal = subTotal,
                Total = total,
            });

            return NoContent();
        }


        // PUT: api/Bills/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutBills(BillStatusDTO query)
        {
            // ORDERING
            // PENDING
            // SUCCESSED
            // DONE

            var user = await _IRepositoryAccounts.GetAsync(e => e.Id == query.userID);
            if (user == null)
            {
                return BadRequest(new Response<BillOutDTO> { Message = "user invalid", Succeeded = false });
            }

            switch (query.status)
            {
                case "ORDERING":
                case "PENDING":
                    {
                        await _IRepositoryBill.UpdateStatusAsync(new Bill { AccountID = query.userID, Status = query.status });
                        return Ok(new Response<BillDetailInDTO> { Succeeded = true });
                    }
                case "SUCCESSED":
                    {
                        await _IRepositoryBill.UpdateStatusAsync(new Bill { AccountID = query.userID, Status = query.status });
                        return Ok(new Response<BillDetailInDTO> { Succeeded = true });
                    }
                case "DONE":
                    {
                        await _IRepositoryBill.UpdateStatusAsync(new Bill { AccountID = query.userID, Status = query.status });
                        return Ok(new Response<BillDetailInDTO> { Succeeded = true });
                    }
                default:
                    {
                        return BadRequest(new Response<BillDetailInDTO> { Message = "status incorrect please choose 'ORDERING || PENDING || SUCCESSED || DONE'", Succeeded = false });
                    }
            }

        }

        // POST: api/Bills
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<BillOutDTO>> PostBill(BillInDTO bill)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(new Response<BillOutDTO> { Message = "value incorrect", Succeeded = false });
        //    }

        //    await _IRepositoryBill.CreateAsync(new Bill
        //    {
        //        AccountID = bill.accountID,
        //        VoucherID = bill.voucherID,
        //        Status = "ORDERING",
        //    });

        //    return CreatedAtAction("GetBill", new { id = bill.Id }, bill);
        //}

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
