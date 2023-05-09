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
using IceCreamBE.DTO;
using IceCreamBE.DTO.PageList;
using System.Drawing.Printing;

namespace IceCreamBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IRepositoryBrand _IRepositoryBrand;

        public BrandsController(IRepositoryBrand IRepositoryBrand)
        {
            _IRepositoryBrand = IRepositoryBrand;
        }

        // GET: api/Brands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandsDTO>>> GetBrands([FromQuery] PaginationFilter<BrandsDTO>? filter)
        {
            var result = await _IRepositoryBrand.GetAllAsync();

            var pageFilter = new PaginationFilter<BrandsDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(result);

            return Ok(new PagedResponse<List<Brands>>
            {
                Data = pagedData,
                Succeeded = true,
                currentPage = pageFilter.PageNumber,
                PageSize = pageFilter.PageSize,
                TotalPages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                TotalRecords = result.Count
            });
        }

        // GET: api/Brands/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BrandsDTO>> GetBrands(int id)
        {
            var result = await _IRepositoryBrand.GetAsync(e => e.Id == id);

            if (result == null)
            {
                return NotFound(new Response<Brands> { Succeeded = false, Message = "Not found" });
            }

            return Ok(new Response<Brands> { Data = result, Succeeded = true });
        }

        // GET: api/Brands/brandname
        [HttpGet("{query}")]
        public async Task<ActionResult<BrandsDTO>> GetBrands(string query)
        {
            var result = await _IRepositoryBrand.GetAllAsync(e => e.BrandName.Contains(query));

            if (result == null)
            {
                return NotFound(new Response<Brands> { Succeeded = false, Message = "Not found" });
            }

            return Ok(new Response<List<Brands>> { Succeeded = true, Data = result });
        }

        // PUT: api/Brands/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrands(int id, BrandsDTO brands)
        {
            if (id != brands.Id)
            {
                return BadRequest();
            }

            var result = await _IRepositoryBrand.GetAsync(e => e.Id == id);

            if (result == null)
            {
                return NotFound(new Response<Brands> { Succeeded = false, Message = "Not found" });
            }

            await _IRepositoryBrand.UpdateAsync(new Brands
            {
                Id = brands.Id,
                BrandName = brands.BrandName
            });

            return NoContent();
        }

        // POST: api/Brands
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BrandsDTO>> PostBrands(BrandsDTO brands)
        {
            var result = await _IRepositoryBrand.GetAsync(e => e.BrandName == brands.BrandName);

            if (result != null)
            {
                return BadRequest(new Response<Brands> { Succeeded = false, Message = brands.BrandName + " is valid" });
            }

            await _IRepositoryBrand.CreateAsync(new Brands
            {
                BrandName = brands.BrandName
            });

            return CreatedAtAction("GetBrands", new { id = brands.Id }, brands);
        }

        // DELETE: api/Brands/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrands(int id)
        {
            var result = await _IRepositoryBrand.GetAsync(e => e.Id == id);

            if (result == null)
            {
                return NotFound(new Response<Brands> { Succeeded = false, Message = "Not found" });
            }

            await _IRepositoryBrand.DeleteAsync(result);

            return NoContent();
        }
    }
}
