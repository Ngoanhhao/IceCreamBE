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

        public VouchersController(IRepositoryVourcher IRepositoryVourcher)
        {
            _IRepositoryVourcher = IRepositoryVourcher;
        }

        //GET: api/Vouchers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VouchersDTO>>> GetVouchers([FromQuery] PaginationFilter<VouchersDTO>? filter)
        {
            var newResult = new List<VouchersDTO>();
            var result = await _IRepositoryVourcher.GetAllAsync();

            result.ForEach(e => newResult.Add(new VouchersDTO
            {
                Id = e.Id,
                adminID = e.AdminID,
                discount = e.Discount,
                expiration_date = e.ExpirationDate,
                status = e.Status,
                voucher = e.Voucher,
            }));

            var pageFilter = new PaginationFilter<VouchersDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(newResult);

            return Ok(new PagedResponse<List<VouchersDTO>>
            {
                Data = pagedData,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<VouchersDTO>>
                {
                    current_page = pagedData == null ? 0 : pageFilter.PageNumber,
                    Page_pize = pagedData == null ? 0 : pageFilter.PageSize,
                    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                    total_records = result.Count
                }
            });
        }

        //// GET: api/Vouchers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VouchersDTO>> GetVouchers(int id)
        {
            var result = await _IRepositoryVourcher.GetAsync(e => e.Id == id);
            if (result == null)
            {
                return NotFound(new Response<List<VouchersDTO>> { Message = "not found", Succeeded = false });
            }

            return Ok(new Response<VouchersDTO>
            {
                Data = new VouchersDTO
                {
                    adminID = result.AdminID,
                    discount = result.Discount,
                    expiration_date = result.ExpirationDate,
                    status = result.Status,
                    Id = result.Id,
                    voucher = result.Voucher,
                },
                Succeeded = true
            });
        }

        // PUT: api/Vouchers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVouchers(int id, VouchersDTO vouchers)
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
        public async Task<ActionResult<Vouchers>> PostVouchers(VouchersDTO vouchers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<List<VouchersDTO>> { Message = "value incorrect", Succeeded = false });
            }

            var voucher = Coupon.CouponGenarate(20);

            await _IRepositoryVourcher.CreateAsync(new Vouchers { 
                AdminID = vouchers.adminID 
                ,Status = vouchers.status,
                Voucher = voucher,
                Discount = vouchers.discount,
                ExpirationDate = DateTime.UtcNow
            });

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
