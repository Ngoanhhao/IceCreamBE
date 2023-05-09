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
                AdminID = e.AdminID,
                Discount = e.Discount,
                ExpirationDate = e.ExpirationDate,
                Status = e.Status,
                Id = e.Id,
                Voucher = e.Voucher,
            }));

            var pageFilter = new PaginationFilter<VouchersDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(newResult);

            return Ok(new PagedResponse<List<VouchersDTO>>
            {
                Data = pagedData,
                Succeeded = true,
                currentPage = pageFilter.PageNumber,
                PageSize = pageFilter.PageSize,
                TotalPages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                TotalRecords = result.Count
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
                    AdminID = result.AdminID,
                    Discount = result.Discount,
                    ExpirationDate = result.ExpirationDate,
                    Status = result.Status,
                    Id = result.Id,
                    Voucher = result.Voucher,
                },
                Succeeded = true
            });
        }
        // test
        //// PUT: api/Vouchers/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutVouchers(int id, Vouchers vouchers)
        //{
        //    if (id != vouchers.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(vouchers).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!VouchersExists(id))
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

        //// POST: api/Vouchers
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Vouchers>> PostVouchers(Vouchers vouchers)
        //{
        //    if (_context.Vouchers == null)
        //    {
        //        return Problem("Entity set 'IceCreamDbcontext.Vouchers'  is null.");
        //    }
        //    _context.Vouchers.Add(vouchers);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetVouchers", new { id = vouchers.Id }, vouchers);
        //}

        //// DELETE: api/Vouchers/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteVouchers(int id)
        //{
        //    if (_context.Vouchers == null)
        //    {
        //        return NotFound();
        //    }
        //    var vouchers = await _context.Vouchers.FindAsync(id);
        //    if (vouchers == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Vouchers.Remove(vouchers);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool VouchersExists(int id)
        //{
        //    return (_context.Vouchers?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
