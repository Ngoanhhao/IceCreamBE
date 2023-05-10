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
using IceCreamBE.Repository;
using Microsoft.CodeAnalysis;
using IceCreamBE.Migrations;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IceCreamBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepositoryProduct _IRepositoryProduct;
        private readonly IRepositoryBrand _IRepositoryBrand;

        public ProductsController(IRepositoryProduct repositoryProduct, IRepositoryBrand iRepositoryBrand)
        {
            _IRepositoryProduct = repositoryProduct;
            _IRepositoryBrand = iRepositoryBrand;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDetailDTO>>> GetProducts([FromQuery] PaginationFilter<ProductDetailDTO>? filter)
        {
            var brand = (await _IRepositoryBrand.GetAllAsync());
            var product = (await _IRepositoryProduct.GetAllAsync());
            var item = new List<ProductDetailDTO>();

            var result = product.Join(brand,
                        r => r.BrandID,
                        p => p.Id,
                        (r, p) => new { product = r, brand = p })
                        .Select(e => new ProductDetailDTO
                        {
                            Id = e.product.Id,
                            brand_name = e.brand.BrandName,
                            cost = e.product.Cost,
                            description = e.product.Description,
                            price = e.product.Price,
                            discount_percent = e.product.Discount,
                            img = e.product.Img,
                            name = e.product.Name,
                            status = e.product.Status,
                            total = e.product.Total
                        }).ToList();

            var pageFilter = new PaginationFilter<ProductDetailDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(result);

            return Ok(new PagedResponse<List<ProductDetailDTO>>
            {
                Data = result,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<ProductDetailDTO>>
                {
                    current_page = pagedData == null ? 0 : pageFilter.PageNumber,
                    Page_pize = pagedData == null ? 0 : pageFilter.PageSize,
                    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                    total_records = result.Count
                }
            });
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDetailDTO>> GetProducts(int id)
        {
            var product = await _IRepositoryProduct.GetAsync(e => e.Id == id);
            var brand = await _IRepositoryBrand.GetAsync(e => e.Id == product.BrandID);

            if (product == null)
            {
                return NotFound(new Response<ProductDetailDTO> { Message = "not found", Succeeded = false });
            }

            return Ok(new Response<ProductDetailDTO>
            {
                Succeeded = true,
                Data = new ProductDetailDTO
                {
                    Id = product.Id,
                    total = product.Total,
                    status = product.Status,
                    name = product.Name,
                    img = product.Img,
                    brand_name = brand.BrandName,
                    cost = product.Cost,
                    description = product.Description,
                    price = product.Price,
                    discount_percent = product.Discount
                }
            });
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducts(int id, ProductsDTO products)
        {
            if (id != products.Id)
            {
                return BadRequest(new Response<ProductDetailDTO> { Message = "value incorrect", Succeeded = false });
            }

            var result = await _IRepositoryProduct.GetAsync(e => e.Id == id);
            if (result == null)
            {
                return NotFound(new Response<ProductDetailDTO> { Message = "not found", Succeeded = false });
            }

            await _IRepositoryProduct.UpdateAsync(new Products
            {
                Id = products.Id,
                Description = products.description,
                BrandID = products.brandID,
                Cost = products.cost,
                Img = products.img,
                Price = products.price,
                Discount = products.discount_percent,
                Total = products.total,
                Status = products.status,
                Name = products.name
            });

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Products>> PostProducts(ProductsDTO products)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<ProductDetailDTO> { Message = "value incorrect", Succeeded = false });
            }

            await _IRepositoryProduct.CreateAsync(new Products
            {
                Description = products.description,
                BrandID = products.brandID,
                Cost = products.cost,
                Img = products.img,
                Price = products.price,
                Discount = products.discount_percent,
                Total = (double)(((100 - products.discount_percent) * 0.01) * products.price),
                Status = products.status,
                Name = products.name
            });

            return CreatedAtAction("GetProducts", new
            {
                id = products.Id
            }, products);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducts(int id)
        {
            var result = await _IRepositoryProduct.GetAsync(e => e.Id == id);
            if (result == null)
            {
                return NotFound(new Response<RecipeOutDTO> { Message = "not found", Succeeded = false });
            }

            await _IRepositoryProduct.DeleteAsync(result);

            return NoContent();
        }
    }
}
