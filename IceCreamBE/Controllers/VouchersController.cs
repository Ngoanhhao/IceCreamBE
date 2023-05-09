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

        // GET: api/Vouchers
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Vouchers>>> GetVouchers()
        //{
        //    if (_context.Vouchers == null)
        //    {
        //        return NotFound();
        //    }
        //    return await _context.Vouchers.ToListAsync();
        //}

        //// GET: api/Vouchers/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Vouchers>> GetVouchers(int id)
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

        //    return vouchers;
        //}

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
