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
using Microsoft.AspNetCore.Http.HttpResults;
using IceCreamBE.Modules;
using IceCreamBE.Migrations;

namespace IceCreamBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VouchersController : ControllerBase
    {
        private readonly IRepositoryVourcher _IRepositoryVourcher;
        private readonly IRepositoryAccounts _IRepositoryAccounts;
        private readonly IRepositoryAccountDetail _IRepositoryAccountsDetails;

        public VouchersController(IRepositoryVourcher IRepositoryVourcher, IRepositoryAccounts IRepositoryAccounts, IRepositoryAccountDetail iRepositoryAccountsDetails)
        {
            _IRepositoryVourcher = IRepositoryVourcher;
            _IRepositoryAccounts = IRepositoryAccounts;
            _IRepositoryAccountsDetails = iRepositoryAccountsDetails;
        }

        //GET: api/Vouchers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VouchersDTO>>> GetVouchers([FromQuery] PaginationFilter<VouchersDTO>? filter)
        {
            var voucher = await _IRepositoryVourcher.GetAllAsync();
            var account = await _IRepositoryAccounts.GetAllAsync();

            var result = voucher.Join(account,
                e => e.AdminID,
                q => q.Id,
                (e, q) => new { voucher = e, account = q })
                .Select(e => new VoucherOutDTO
                {
                    Id = e.voucher.Id,
                    discount_percent = e.voucher.Discount,
                    ExpirationDate = e.voucher.ExpirationDate,
                    status = e.voucher.Status,
                    user_name = e.account.Username
                }).ToList();


            var pageFilter = new PaginationFilter<VoucherOutDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(result);

            return Ok(new PagedResponse<List<VoucherOutDTO>>
            {
                Data = pagedData,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<VoucherOutDTO>>
                {
                    current_page = pagedData == null ? 0 : pageFilter.PageNumber,
                    Page_pize = pagedData == null ? 0 : pageFilter.PageSize,
                    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                    total_records = result.Count
                }
            });
        }

        //// GET: api/Vouchers/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<VoucherOutDTO>> GetVouchers(int id)
        {
            var vouchers = await _IRepositoryVourcher.GetAsync(e => e.Id == id);
            if (vouchers == null)
            {
                return NotFound(new Response<List<VoucherOutDTO>> { Message = "not found", Succeeded = false });
            }
            var accounts = await _IRepositoryAccounts.GetAsync(e => e.Id == vouchers.AdminID);
            if (accounts == null)
            {
                return NotFound(new Response<List<VoucherOutDTO>> { Message = "not found", Succeeded = false });
            }

            return Ok(new Response<VoucherOutDTO>
            {
                Data = new VoucherOutDTO
                {
                    Id = vouchers.Id,
                    discount_percent = vouchers.Discount,
                    ExpirationDate = vouchers.ExpirationDate,
                    status = vouchers.Status,
                    user_name = accounts.Username
                },
                Succeeded = true
            });
        }

        //// GET: api/Vouchers/5
        [HttpGet("{voucher}")]
        public async Task<ActionResult<VoucherOutDTO>> GetVouchers(string voucher)
        {
            var vouchers = await _IRepositoryVourcher.GetAsync(e => e.Voucher == voucher);
            if (vouchers == null)
            {
                return NotFound(new Response<List<VoucherOutDTO>> { Message = "not found", Succeeded = false });
            }
            var accounts = await _IRepositoryAccounts.GetAsync(e => e.Id == vouchers.AdminID);
            if (accounts == null)
            {
                return NotFound(new Response<List<VoucherOutDTO>> { Message = "not found", Succeeded = false });
            }

            return Ok(new Response<VoucherOutDTO>
            {
                Data = new VoucherOutDTO
                {
                    Id = vouchers.Id,
                    discount_percent = vouchers.Discount,
                    ExpirationDate = vouchers.ExpirationDate,
                    status = vouchers.Status,
                    user_name = accounts.Username
                },
                Succeeded = true
            });
        }


        // PUT: api/Vouchers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVouchers(int id, VoucherInDTO vouchers)
        {
            if (id != vouchers.Id)
            {
                return BadRequest(new Response<List<VouchersDTO>> { Message = "value incorrect", Succeeded = false });
            }

            var result = await _IRepositoryVourcher.GetAsync(e => e.Id == id);
            if (result == null)
            {
                return NotFound(new Response<List<VouchersDTO>> { Message = "not found", Succeeded = false });
            }

            await _IRepositoryVourcher.UpdateAsync(new Vouchers { Id = vouchers.Id, Status = vouchers.status });

            return NoContent();
        }

        // POST: api/Vouchers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Vouchers>> PostVouchers(VoucherInDTO vouchers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<List<VouchersDTO>> { Message = "value incorrect", Succeeded = false });
            }

            if ((vouchers.hourExpiration <= 0 || vouchers.hourExpiration == null) && vouchers.ExpirationDate == null)
            {
                return BadRequest(new Response<List<VouchersDTO>> { Message = "hourExpiration or ExpirationDate incorrect", Succeeded = false });
            }

            var user = await _IRepositoryAccounts.GetAsync(e => e.Id == vouchers.adminID);
            if (user == null)
            {
                return BadRequest(new Response<List<VouchersDTO>> { Message = "user incorrect", Succeeded = false });
            }

            var voucher = Coupon.CouponGenarate(20);

            if (vouchers.hourExpiration > 0)
            {
                await _IRepositoryVourcher.CreateAsync(new Vouchers
                {
                    AdminID = vouchers.adminID,
                    Status = vouchers.status,
                    Voucher = voucher,
                    Discount = vouchers.discount_percent,
                    ExpirationDate = DateTime.Now.AddHours((double)vouchers.hourExpiration)
                });
            }
            else if (vouchers.ExpirationDate != null)
            {
                await _IRepositoryVourcher.CreateAsync(new Vouchers
                {
                    AdminID = vouchers.adminID,
                    Status = vouchers.status,
                    Voucher = voucher,
                    Discount = vouchers.discount_percent,
                    ExpirationDate = (DateTime)vouchers.ExpirationDate == null ? DateTime.Now.AddHours(1) : (DateTime)vouchers.ExpirationDate
                });
            }


            return NotFound(new Response<string> { Data = voucher, Succeeded = true });
        }

        // DELETE: api/Vouchers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVouchers(int id)
        {
            var result = await _IRepositoryVourcher.GetAsync(e => e.Id == id);
            if (result == null)
            {
                return NotFound(new Response<List<VouchersDTO>> { Message = "not found", Succeeded = false });
            }

            await _IRepositoryVourcher.DeleteAsync(result);

            return NoContent();
        }
    }
}
