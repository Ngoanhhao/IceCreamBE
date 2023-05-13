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
using System.Data;

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
            var roles = await _repository.GetAllAsync();
            var result = roles.Select(e => new RolesDTO
            {
                Id = e.Id,
                Role = e.Role
            }).ToList();
            var pageFilter = new PaginationFilter<RolesDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(result);

            return Ok(new PagedResponse<List<RolesDTO>>
            {
                Data = pagedData,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<RolesDTO>>
                {
                    current_page = pagedData == null ? 0 : pageFilter.PageNumber,
                    Page_pize = pagedData == null ? 0 : pageFilter.PageSize,
                    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                    total_records = result.Count
                }
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
            return Ok(new Response<RolesDTO>
            {
                Succeeded = true,
                Data = new RolesDTO { Id = item.Id, Role = item.Role }
            });
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoles(int id, RolesDTO roles)
        {
            if (id != roles.Id)
            {
                return BadRequest(new Response<RolesDTO> { Succeeded = false, Message = "value incorrect" });
            }

            if (await _repository.GetAsync(e => e.Id == id) != null)
            {
                await _repository.UpdateAsync(new Roles
                {
                    Id = roles.Id,
                    Role = roles.Role
                });
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
            await _repository.CreateAsync(new Roles
            {
                Id = roles.Id,
                Role = roles.Role
            });

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
