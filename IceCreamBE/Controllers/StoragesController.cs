using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IceCreamBE.Data;
using IceCreamBE.Models;
using IceCreamBE.Migrations;
using IceCreamBE.Repository.Irepository;
using IceCreamBE.DTO;
using IceCreamBE.DTO.PageList;

namespace IceCreamBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoragesController : ControllerBase
    {
        private readonly IRepositoryStorage _IRepositoryStorage;
        private readonly IRepositoryProduct _IRepositoryProduct;
        private readonly IRepositoryBrand _IRepositoryBrand;

        public StoragesController(IRepositoryStorage IRepositoryStorage, IRepositoryProduct iRepositoryProduct, IRepositoryBrand iRepositoryBrand)
        {
            _IRepositoryStorage = IRepositoryStorage;
            _IRepositoryProduct = iRepositoryProduct;
            _IRepositoryBrand = iRepositoryBrand;
        }

        //GET: api/Storages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StorageOutDTO>>> Getstorage([FromQuery] PaginationFilter<StorageOutDTO>? filter)
        {
            var storage = (await _IRepositoryStorage.GetAllAsync()).AsQueryable<Storage>();
            var product = (await _IRepositoryProduct.GetAllAsync()).AsQueryable<Products>();
            var result = storage
                .Join(product,
                    s => s.ProductID,
                    p => p.Id,
                    (s, p) => new { storage = s, product = p })
                .Select(e => new StorageOutDTO
                {
                    Id = e.storage.ProductID,
                    product_name = e.product.Name,
                    last_order = e.storage.LastOrder,
                    quantity = e.storage.Quantity,
                }).ToList();
            var pageFilter = new PaginationFilter<StorageOutDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(result.ToList());

            return Ok(new PagedResponse<List<StorageOutDTO>>
            {
                Data = pagedData,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<StorageOutDTO>>
                {
                    current_page = pagedData == null ? 0 : pageFilter.PageNumber,
                    Page_pize = pagedData == null ? 0 : pageFilter.PageSize,
                    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                    total_records = result.Count
                }
            });
        }

        // GET: api/Storages/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<StorageOutDTO>> GetStorage(int id)
        {
            var storage = (await _IRepositoryStorage.GetAsync(e => e.ProductID == id));
            if (storage == null)
            {
                return NotFound(new Response<StorageOutDTO> { Message = "not found", Succeeded = false });
            }

            var product = (await _IRepositoryProduct.GetAsync(e => e.Id == storage.ProductID));

            var result = new StorageOutDTO
            {
                Id = storage.ProductID,
                product_name = product.Name,
                last_order = storage.LastOrder,
                quantity = storage.Quantity,
            };

            return Ok(new Response<StorageOutDTO> { Data = result, Succeeded = true });
        }


        // GET: api/Storages/productname
        [HttpGet("{query}")]
        public async Task<ActionResult<StorageOutDTO>> GetStorage(string query)
        {
            var storage = (await _IRepositoryStorage.GetAllAsync()).AsQueryable<Storage>();
            var product = (await _IRepositoryProduct.GetAllAsync()).AsQueryable<Products>();
            var result = storage
                .Join(product,
                    s => s.ProductID,
                    p => p.Id,
                    (s, p) => new { storage = s, product = p })
                .Where(e => e.product.Name.Contains(query))
                .Select(e => new StorageOutDTO
                {
                    Id = e.storage.ProductID,
                    product_name = e.product.Name,
                    last_order = e.storage.LastOrder,
                    quantity = e.storage.Quantity,
                });
            if (result.Count() == 0)
            {
                return NotFound(new Response<List<StorageOutDTO>> { Message = "not found", Succeeded = false });
            }
            return Ok(new Response<List<StorageOutDTO>> { Data = result.ToList(), Succeeded = true });
        }

        // PUT: api/Storages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStorage(int id, int quantity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<List<StorageOutDTO>> { Message = "value incorrect", Succeeded = false });
            }

            var result = await _IRepositoryStorage.GetAsync(e => e.ProductID == id);
            if (result == null)
            {
                return NotFound(new Response<List<StorageOutDTO>> { Message = "not found", Succeeded = false });
            }

            await _IRepositoryStorage.UpdateAsync(id, quantity);

            return NoContent();
        }

        // POST: api/Storages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StorageOutDTO>> PostStorage(StorageInDTO storage)
        {
            var product = await _IRepositoryStorage.GetAsync(e => e.ProductID == storage.ProductId);
            if (product != null)
            {
                return BadRequest(new Response<List<StorageOutDTO>> { Message = "product is available", Succeeded = false });
            }

            await _IRepositoryStorage.CreateAsync(new Storage
            {
                ProductID = storage.ProductId,
                Quantity = storage.quantity,
                LastOrder = DateTime.UtcNow,
            });
            return Ok(new Response<List<StorageOutDTO>> { Succeeded = true });
        }
    }
}
