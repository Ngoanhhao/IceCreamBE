﻿using System;
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
        private readonly IRepositoryProduct _IRepositoryProduct;

        public BrandsController(IRepositoryBrand IRepositoryBrand, IRepositoryProduct iRepositoryProduct)
        {
            _IRepositoryBrand = IRepositoryBrand;
            _IRepositoryProduct = iRepositoryProduct;
        }

        // GET: api/Brands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandsDTO>>> GetBrands([FromQuery] PaginationFilter<BrandsDTO>? filter)
        {
            var result = await _IRepositoryBrand.GetAllAsync();
            var item = new List<BrandsDTO>();
            var product = await _IRepositoryProduct.GetAllAsync();
            result.ForEach(e =>
            {
                item.Add(new BrandsDTO { Id = e.Id, brand_name = e.BrandName, product_count = product.Where(q => q.BrandID == e.Id).Count() });
            });

            var pageFilter = new PaginationFilter<BrandsDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(item);

            return Ok(new PagedResponse<List<BrandsDTO>>
            {
                Data = pagedData,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<BrandsDTO>>
                {
                    current_page = pageFilter.PageNumber,
                    Page_pize = pageFilter.PageSize,
                    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                    total_records = result.Count
                }
            });
        }

        // GET: api/Brands/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BrandsDTO>> GetBrand(int id)
        {
            var result = await _IRepositoryBrand.GetAsync(e => e.Id == id);
            if (result == null)
            {
                return NotFound(new Response<BrandsDTO> { Succeeded = false, Message = "Not found" });
            }

            var product = await _IRepositoryProduct.GetAllAsync(e => e.BrandID == result.Id);

            return Ok(new Response<BrandsDTO>
            {
                Data = new BrandsDTO
                {
                    Id = result.Id,
                    brand_name = result.BrandName,
                    product_count = product.Count()
                }
                ,
                Succeeded = true
            });
        }

        // GET: api/Brands/brandname
        [HttpGet("{query}")]
        public async Task<ActionResult<BrandsDTO>> GetBrands([FromQuery] PaginationFilter<BrandsDTO>? filter, string query)
        {
            var result = await _IRepositoryBrand.GetAllAsync(e => e.BrandName.Contains(query));
            var item = new List<BrandsDTO>();
            var product = await _IRepositoryProduct.GetAllAsync();
            result.ForEach(e =>
            {
                item.Add(new BrandsDTO { Id = e.Id, brand_name = e.BrandName, product_count = product.Where(q => q.BrandID == e.Id).Count() });
            });

            var pageFilter = new PaginationFilter<BrandsDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(item);

            return Ok(new PagedResponse<List<BrandsDTO>>
            {
                Data = pagedData,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<BrandsDTO>>
                {
                    current_page = pageFilter.PageNumber,
                    Page_pize = pageFilter.PageSize,
                    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                    total_records = result.Count
                }
            });
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
                BrandName = brands.brand_name
            });

            return NoContent();
        }

        // POST: api/Brands
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BrandsDTO>> PostBrands(BrandsDTO brands)
        {
            var result = await _IRepositoryBrand.GetAsync(e => e.BrandName == brands.brand_name);

            if (result != null)
            {
                return BadRequest(new Response<Brands> { Succeeded = false, Message = brands.brand_name + " is valid" });
            }

            await _IRepositoryBrand.CreateAsync(new Brands
            {
                BrandName = brands.brand_name
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
