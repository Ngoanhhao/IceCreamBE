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
using Microsoft.AspNetCore.Authorization;

namespace IceCreamBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepositoryProduct _IRepositoryProduct;
        private readonly IRepositoryBrand _IRepositoryBrand;
        private readonly IRepositoryFileService _IRepositoryFileService;
        private readonly IRepositoryStorage _IRepositoryStorage;

        public ProductsController(IRepositoryProduct repositoryProduct, IRepositoryBrand iRepositoryBrand, IRepositoryFileService iRepositoryFileService, IRepositoryStorage iRepositoryStorage)
        {
            _IRepositoryProduct = repositoryProduct;
            _IRepositoryBrand = iRepositoryBrand;
            _IRepositoryFileService = iRepositoryFileService;
            _IRepositoryStorage = iRepositoryStorage;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductOutDTO>>> GetProducts([FromQuery] PaginationFilter<ProductsInDTO>? filter)
        {
            var brand = (await _IRepositoryBrand.GetAllAsync());
            var product = (await _IRepositoryProduct.GetAllAsync());
            string url = $"{Request.Scheme}://{Request.Host}/api/image/";

            var result = product.Join(brand,
                        r => r.BrandID,
                        p => p.Id,
                        (r, p) => new { product = r, brand = p })
                        .Select(e => new ProductOutDTO
                        {
                            Id = e.product.Id,
                            brand_name = e.brand.BrandName,
                            cost = e.product.Cost,
                            description = e.product.Description,
                            price = e.product.Price,
                            discount_percent = e.product.Discount,
                            img = _IRepositoryFileService.CheckImage(e.product.Img, "Images") ? url + e.product.Img : null,
                            name = e.product.Name,
                            status = e.product.Status,
                            total = e.product.Total
                        }).ToList();
            foreach (var item in result)
            {
                var quantity = await _IRepositoryStorage.GetAsync(e => e.ProductID == item.Id);
                item.quantity = quantity == null ? 0 : quantity.Quantity;
            }

            var pageFilter = new PaginationFilter<ProductOutDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(result);

            return Ok(new PagedResponse<List<ProductOutDTO>>
            {
                Data = pagedData,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<ProductOutDTO>>
                {
                    current_page = pagedData == null ? 0 : pageFilter.PageNumber,
                    Page_pize = pagedData == null ? 0 : pageFilter.PageSize,
                    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                    total_records = result.Count
                }
            });
        }

        // GET: api/ProductName
        [HttpGet("/api/search/product")]
        public async Task<ActionResult<IEnumerable<ProductOutDTO>>> GetProduct([FromQuery] PaginationFilter<ProductsInDTO>? filter, [FromQuery] string? query)
        {
            var brand = (await _IRepositoryBrand.GetAllAsync());
            var product = (await _IRepositoryProduct.GetAllAsync());
            string url = $"{Request.Scheme}://{Request.Host}/api/image/";

            var result = product.Join(brand,
                        r => r.BrandID,
                        p => p.Id,
                        (r, p) => new { product = r, brand = p })
                        .Select(e => new ProductOutDTO
                        {
                            Id = e.product.Id,
                            brand_name = e.brand.BrandName,
                            cost = e.product.Cost,
                            description = e.product.Description,
                            price = e.product.Price,
                            discount_percent = e.product.Discount,
                            img = _IRepositoryFileService.CheckImage(e.product.Img, "Images") ? url + e.product.Img : null,
                            name = e.product.Name,
                            status = e.product.Status,
                            total = e.product.Total
                        }).Where(e => e.name.ToLower().Contains(query != null ? query.ToLower() : "")).ToList();

            foreach (var item in result)
            {
                var quantity = await _IRepositoryStorage.GetAsync(e => e.ProductID == item.Id);
                item.quantity = quantity == null ? 0 : quantity.Quantity;
            }

            var pageFilter = new PaginationFilter<ProductOutDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(result);

            return Ok(new PagedResponse<List<ProductOutDTO>>
            {
                Data = pagedData,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<ProductOutDTO>>
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
        public async Task<ActionResult<ProductOutDTO>> GetProducts(int id)
        {
            var product = await _IRepositoryProduct.GetAsync(e => e.Id == id);
            var brand = await _IRepositoryBrand.GetAsync(e => e.Id == product.BrandID);
            string url = $"{Request.Scheme}://{Request.Host}/api/image/";

            if (product == null)
            {
                return NotFound(new Response<ProductOutDTO> { Message = "not found", Succeeded = false });
            }
            var quantity = await _IRepositoryStorage.GetAsync(e => e.ProductID == product.Id);



            return Ok(new Response<ProductOutDTO>
            {
                Succeeded = true,
                Data = new ProductOutDTO
                {
                    Id = product.Id,
                    total = product.Total,
                    status = product.Status,
                    name = product.Name,
                    quantity = quantity == null ? 0 : quantity.Quantity,
                    img = _IRepositoryFileService.CheckImage(product.Img, "Images") ? url + product.Img : null,
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutProducts(int id, ProductsInDTO products)
        {
            if (id != products.Id)
            {
                return BadRequest(new Response<ProductOutDTO> { Message = "value incorrect", Succeeded = false });
            }

            var result = await _IRepositoryProduct.GetAsync(e => e.Id == id);
            if (result == null)
            {
                return NotFound(new Response<ProductOutDTO> { Message = "not found", Succeeded = false });
            }
            var brand = await _IRepositoryBrand.GetAsync(e => e.Id == products.brandID);
            if (brand == null)
            {
                return BadRequest(new Response<ProductOutDTO> { Message = "brand is valid", Succeeded = false });
            }

            var discount = products.discount_percent >= 0 ? products.discount_percent : 0;

            await _IRepositoryProduct.UpdateAsync(new Products
            {
                Id = products.Id,
                Description = products.description,
                BrandID = products.brandID,
                Cost = products.cost,
                Img = products.img,
                Discount = discount,
                Price = products.price,
                Total = (double)((100 - discount) * 0.01) * products.price,
                Status = products.status,
                Name = products.name
            });

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Products>> PostProducts(ProductsInDTO products)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<ProductOutDTO> { Message = "value incorrect", Succeeded = false });
            }
            var brand = await _IRepositoryBrand.GetAsync(e => e.Id == products.brandID);
            if (brand == null)
            {
                return BadRequest(new Response<ProductOutDTO> { Message = "brand is valid", Succeeded = false });
            }

            var discount = products.discount_percent >= 0 ? products.discount_percent : 0;

            await _IRepositoryProduct.CreateAsync(new Products
            {
                Description = products.description,
                BrandID = products.brandID,
                Cost = products.cost,
                Img = products.img,
                Price = products.price,
                Discount = discount,
                Total = (double)(((100 - discount) * 0.01) * products.price),
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
        [Authorize(Roles = "Admin")]
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
