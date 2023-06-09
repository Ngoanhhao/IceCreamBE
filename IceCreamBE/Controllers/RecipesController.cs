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
using IceCreamBE.Migrations;
using IceCreamBE.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Policy;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        public async Task<ActionResult<IEnumerable<RecipeOutDTO>>> GetRecipe([FromQuery] PaginationFilter<RecipeOutDTO>? filter)
        {
            var recipe = (await _IRepositoryRecipe.GetAllAsync());

            string url = $"{Request.Scheme}://{Request.Host}/api/image/";

            var result = recipe.Select(e => new RecipeOutDTO
            {
                Id = e.Id,
                description = e.Description,
                img = _IRepositoryFileService.CheckImage(e.img, "Images") ? url + e.img : null,
                name = e.Name,
                status = e.Status,
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
        [Authorize]
        public async Task<ActionResult<RecipeOutDTO>> GetRecipe(int id)
        {
            var result = await _IRepositoryRecipe.GetAsync(e => e.Id == id);
            string url = $"{Request.Scheme}://{Request.Host}/api/image/";

            if (result == null)
            {
                return BadRequest(new Response<RecipeOutDTO> { Message = "not found", Succeeded = false });
            }

            return Ok(new Response<RecipeOutDTO>
            {
                Succeeded = true,
                Data = new RecipeOutDTO
                {
                    Id = result.Id,
                    description = result.Description,
                    name = result.Name,
                    status = result.Status,
                    img = _IRepositoryFileService.CheckImage(result.img, "Images") ? url + result.img : null,
                }
            });
        }


        // GET: api/Recipes/product_name
        [HttpGet("/api/search/recipes")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<RecipeOutDTO>> SearchRecipe([FromQuery] PaginationFilter<RecipeOutDTO>? filter, [FromQuery] string? query)
        {
            var recipe = await _IRepositoryRecipe.GetAllAsync(e => e.Name.ToLower().Contains(query != null ? query.ToLower() : ""));

            string url = $"{Request.Scheme}://{Request.Host}/api/image/";

            var result = recipe.Select(e => new RecipeOutDTO
            {
                Id = e.Id,
                description = e.Description,
                img = _IRepositoryFileService.CheckImage(e.img, "Images") ? url + e.img : null,
                name = e.Name,
                status = e.Status,
            }).ToList();

            if (result == null)
            {
                return BadRequest(new Response<RecipeInDTO> { Message = "not found", Succeeded = false });
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
                    total_pages = (int)Math.Ceiling((double)result.ToList().Count / (double)filter.PageSize),
                    total_records = result.ToList().Count
                }
            });
        }


        // PUT: api/Recipes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutRecipe(int id, RecipeInDTO recipe)
        {
            if (id != recipe.Id)
            {
                return BadRequest(new Response<RecipeInDTO> { Message = "value incorrect", Succeeded = false });
            }

            var result = await _IRepositoryRecipe.GetAsync(e => e.Id == id);
            if (result == null)
            {
                return BadRequest(new Response<RecipeInDTO> { Message = "not found", Succeeded = false });
            }

            await _IRepositoryRecipe.UpdateAsync(new Recipe
            {
                Id = result.Id,
                Description = recipe.description,
                img = recipe.img,
                Name = recipe.name,
                Status = recipe.status,
            });

            return NoContent();
        }

        // POST: api/Recipes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<RecipeOutDTO>> PostRecipe(RecipeInDTO recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<FeedbackDetailDTO> { Message = "value incorrect", Succeeded = false });
            }


            await _IRepositoryRecipe.CreateAsync(new Recipe
            {
                Name = recipe.name,
                Description = recipe.description,
                Status = recipe.status,
                img = recipe.img,
            });


            return CreatedAtAction("GetRecipe", new { id = recipe.Id }, recipe);
        }

        // DELETE: api/Recipes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var result = await _IRepositoryRecipe.GetAsync(e => e.Id == id);
            if (result == null)
            {
                return BadRequest(new Response<RecipeOutDTO> { Message = "not found", Succeeded = false });
            }

            await _IRepositoryRecipe.DeleteAsync(result);

            return NoContent();
        }
    }
}
