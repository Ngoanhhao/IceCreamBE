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
using Microsoft.AspNetCore.Http.HttpResults;

namespace IceCreamBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRepositoryRoles _repository;

        public RolesController(IRepositoryRoles repository)
        {
            _repository = repository;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult> GetRoles([FromQuery] PaginationFilter<RolesDTO>? filter)
        {
            var result = (await _repository.GetAllAsync()).ToList();
            var pageFilter = new PaginationFilter<RolesDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(result);

            return Ok(new PagedResponse<List<RolesDTO>>
            {
                Data = pagedData,
                Succeeded = true,
                currentPage = pageFilter.PageNumber,
                PageSize = pageFilter.PageSize,
                TotalPages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                TotalRecords = result.Count
            });
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetRoles(int id)
        {
            var item = await _repository.GetAsync(e => e.Id == id);
            if (item == null)
            {
                return NotFound(new Response<Brands> { Succeeded = false, Message = "Not found" });
            }
            return Ok(new Response<RolesDTO> { Succeeded = true, Data = item });
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoles(int id, RolesDTO roles)
        {
            if (id != roles.Id)
            {
                return BadRequest(new Response<Brands> { Succeeded = false, Message = "value incorrect" });
            }

            if (await _repository.GetAsync(e => e.Id == id) != null)
            {
                await _repository.UpdateAsync(roles);
                return NoContent();
            }

            return NotFound(new Response<Brands> { Succeeded = false, Message = "Not found" });
        }

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostRoles(RolesDTO roles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _repository.CreateAsync(roles);

            return CreatedAtAction("GetRoles", new { id = roles.Id }, roles);
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoles(int id)
        {
            var item = await _repository.GetAsync(e => e.Id == id);
            if (item == null)
            {
                return NotFound(new Response<Brands> { Succeeded = false, Message = "Not found" });
            }
            await _repository.DeleteAsync(item);

            return NoContent();
        }
    }
}
