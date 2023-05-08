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
        public async Task<ActionResult<IEnumerable<StorageDTO>>> Getstorage()
        {
            var storage = (await _IRepositoryStorage.GetAllAsync()).AsQueryable<Storage>();
            var product = (await _IRepositoryProduct.GetAllAsync()).AsQueryable<Products>();
            var brand = (await _IRepositoryBrand.GetAllAsync()).AsQueryable<Brands>();
            var result = storage
                .Join(product,
                    s => s.ProductID,
                    p => p.Id,
                    (s, p) => new { storage = s, product = p })
                .Join(brand,
                    s => s.product.BrandID,
                    b => b.Id,
                    (s, b) => new { storage = s.storage, product = s.product, brand = b })
                .Select(e => new StorageDTO
                {
                    Id = e.storage.ProductID,
                    ProductName = e.product.Name,
                    Brand = e.brand.BrandName,
                    Img = e.product.Img,
                    LastOrder = e.storage.LastOrder,
                    Quantity = e.storage.Quantity,
                });
            return Ok(result);
        }

        // GET: api/Storages/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<StorageDTO>> GetStorage(int id)
        {
            var storage = (await _IRepositoryStorage.GetAllAsync()).AsQueryable<Storage>();
            var product = (await _IRepositoryProduct.GetAllAsync()).AsQueryable<Products>();
            var brand = (await _IRepositoryBrand.GetAllAsync()).AsQueryable<Brands>();
            var result = storage
                .Join(product,
                    s => s.ProductID,
                    p => p.Id,
                    (s, p) => new { storage = s, product = p })
                .Join(brand,
                    s => s.product.BrandID,
                    b => b.Id,
                    (s, b) => new { storage = s.storage, product = s.product, brand = b })
                .Where(e => e.storage.ProductID == id)
                .Select(e => new StorageDTO
                {
                    Id = e.storage.ProductID,
                    ProductName = e.product.Name,
                    Brand = e.brand.BrandName,
                    Img = e.product.Img,
                    LastOrder = e.storage.LastOrder,
                    Quantity = e.storage.Quantity,
                });
            return Ok(result);
        }


        // GET: api/Storages/productname
        [HttpGet("{query}")]
        public async Task<ActionResult<StorageDTO>> GetStorage(string query)
        {
            var storage = (await _IRepositoryStorage.GetAllAsync()).AsQueryable<Storage>();
            var product = (await _IRepositoryProduct.GetAllAsync()).AsQueryable<Products>();
            var brand = (await _IRepositoryBrand.GetAllAsync()).AsQueryable<Brands>();
            var result = storage
                .Join(product,
                    s => s.ProductID,
                    p => p.Id,
                    (s, p) => new { storage = s, product = p })
                .Join(brand,
                    s => s.product.BrandID,
                    b => b.Id,
                    (s, b) => new { storage = s.storage, product = s.product, brand = b })
                .Where(e => e.product.Name.Contains(query) || e.brand.BrandName.Contains(query))
                .Select(e => new StorageDTO
                {
                    Id = e.storage.ProductID,
                    ProductName = e.product.Name,
                    Brand = e.brand.BrandName,
                    Img = e.product.Img,
                    LastOrder = e.storage.LastOrder,
                    Quantity = e.storage.Quantity,
                });
            return Ok(result);
        }

        // PUT: api/Storages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStorage(int id, int quantity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _IRepositoryStorage.GetAsync(e => e.ProductID == id);
            if (result == null)
            {
                return NotFound();
            }

            await _IRepositoryStorage.UpdateAsync(id, quantity);

            return NoContent();
        }

        // POST: api/Storages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StorageDTO>> PostStorage(StorageDTOCreate storage)
        {
            await _IRepositoryStorage.CreateAsync(new Storage
            {
                ProductID = storage.ProductId,
                Quantity = storage.Quantity,
                LastOrder = DateTime.UtcNow
            });
            return Ok();
        }
    }
}
