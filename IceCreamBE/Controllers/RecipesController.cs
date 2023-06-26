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
using IceCreamBE.Migrations;
using IceCreamBE.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Policy;

namespace IceCreamBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IRepositoryRecipe _IRepositoryRecipe;
        private readonly IRepositoryProduct _IRepositoryProduct;
        private readonly IRepositoryFileService _IRepositoryFileService;

        public RecipesController(IRepositoryRecipe IRepositoryRecipe, IRepositoryProduct iRepositoryProduct, IRepositoryFileService iRepositoryFileService)
        {
            _IRepositoryRecipe = IRepositoryRecipe;
            _IRepositoryProduct = iRepositoryProduct;
            _IRepositoryFileService = iRepositoryFileService;
        }

        // GET: api/Recipes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeOutDTO>>> GetRecipe([FromQuery] PaginationFilter<RecipeOutDTO>? filter)
        {
            var recipe = (await _IRepositoryRecipe.GetAllAsync());
            var product = (await _IRepositoryProduct.GetAllAsync());
            string url = $"{Request.Scheme}://{Request.Host}/api/image/";

            var result = recipe.Join(product,
                        r => r.ProductId,
                        p => p.Id,
                        (r, p) => new { product = p, recipe = r })
                        .Select(e => new RecipeOutDTO
                        {
                            Id = e.recipe.Id,
                            product_name = e.product.Name,
                            description = e.recipe.Description,
                            status = e.recipe.Status,
                            img = _IRepositoryFileService.CheckImage(e.product.Img, "Images") ? url + e.product.Img : null,
                        }).ToList();

            var pageFilter = new PaginationFilter<RecipeOutDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(result);

            return Ok(new PagedResponse<List<RecipeOutDTO>>
            {
                Data = pagedData,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<RecipeOutDTO>>
                {
                    current_page = pagedData == null ? 0 : pageFilter.PageNumber,
                    Page_pize = pagedData == null ? 0 : pageFilter.PageSize,
                    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                    total_records = result.Count
                }
            });
        }

        // GET: api/Recipes/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<RecipeOutDTO>> GetRecipe(int id)
        {
            var result = await _IRepositoryRecipe.GetAsync(e => e.Id == id);
            var product = await _IRepositoryProduct.GetAsync(e => e.Id == result.ProductId);
            string url = $"{Request.Scheme}://{Request.Host}/api/image/";

            if (result == null)
            {
                return NotFound(new Response<RecipeOutDTO> { Message = "not found", Succeeded = false });
            }

            return Ok(new Response<RecipeOutDTO>
            {
                Succeeded = true,
                Data = new RecipeOutDTO
                {
                    Id = result.Id,
                    description = result.Description,
                    product_name = product.Name,
                    status = result.Status,
                    img = _IRepositoryFileService.CheckImage(product.Img, "Images") ? url + product.Img : null,
                }
            });
        }


        // GET: api/Recipes/product_name
        [HttpGet("{query}")]
        public async Task<ActionResult<RecipeOutDTO>> GetRecipe(string query, [FromQuery] PaginationFilter<RecipeOutDTO>? filter)
        {
            var recipe = (await _IRepositoryRecipe.GetAllAsync());
            var product = (await _IRepositoryProduct.GetAllAsync());

            var result = recipe.Join(product,
                        r => r.ProductId,
                        p => p.Id,
                        (r, p) => new { product = p, recipe = r })
                        .Select(e => new RecipeOutDTO
                        {
                            Id = e.recipe.Id,
                            product_name = e.product.Name,
                            description = e.recipe.Description,
                            status = e.recipe.Status
                        })
                        .Where(e => e.product_name.Contains(query)).ToList();
            if (result.Count == 0)
            {
                return NotFound(new Response<RecipeInDTO> { Message = "not found", Succeeded = false });
            }

            var pageFilter = new PaginationFilter<RecipeOutDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(result);

            return Ok(new PagedResponse<List<RecipeOutDTO>>
            {
                Data = pagedData,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<RecipeOutDTO>>
                {
                    current_page = pagedData == null ? 0 : pageFilter.PageNumber,
                    Page_pize = pagedData == null ? 0 : pageFilter.PageSize,
                    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                    total_records = result.Count
                }
            });
        }


        // PUT: api/Recipes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipe(int id, RecipeInDTO recipe)
        {
            if (id != recipe.Id)
            {
                return BadRequest(new Response<RecipeInDTO> { Message = "value incorrect", Succeeded = false });
            }

            var result = await _IRepositoryRecipe.GetAsync(e => e.Id == id);
            if (result == null)
            {
                return NotFound(new Response<RecipeInDTO> { Message = "not found", Succeeded = false });
            }

            await _IRepositoryRecipe.UpdateAsync(new Recipe
            {
                Id = result.Id,
                Description = recipe.description,
                Status = recipe.status,
            });

            return NoContent();
        }

        // POST: api/Recipes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RecipeOutDTO>> PostRecipe(RecipeInDTO recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<FeedbackDetailDTO> { Message = "value incorrect", Succeeded = false });
            }

            var product = await _IRepositoryProduct.GetAsync(e => e.Id == recipe.productID);
            if (product == null)
            {
                return BadRequest(new Response<FeedbackDetailDTO> { Message = "product incorrect", Succeeded = false });
            }

            var recipeCheck = await _IRepositoryRecipe.GetAsync(e => e.ProductId == product.Id);
            if (recipeCheck != null)
            {
                return BadRequest(new Response<List<StorageOutDTO>> { Message = "product is available", Succeeded = false });
            }
            else
            {
                await _IRepositoryRecipe.CreateAsync(new Recipe
                {
                    ProductId = recipe.productID,
                    Description = recipe.description,
                    Status = recipe.status,
                });
            }

            return CreatedAtAction("GetRecipe", new { id = recipe.Id }, recipe);
        }

        // DELETE: api/Recipes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var result = await _IRepositoryRecipe.GetAsync(e => e.Id == id);
            if (result == null)
            {
                return NotFound(new Response<RecipeOutDTO> { Message = "not found", Succeeded = false });
            }

            await _IRepositoryRecipe.DeleteAsync(result);

            return NoContent();
        }
    }
}
