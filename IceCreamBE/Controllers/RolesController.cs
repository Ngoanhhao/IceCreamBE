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
        public async Task<ActionResult> GetRoles()
        {
            return Ok(await _repository.GetAllAsync());
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetRoles(int id)
        {
            var item = await _repository.GetAsync(e => e.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoles(int id, RolesDTO roles)
        {
            if (id != roles.Id)
            {
                return BadRequest();
            }

            if (await _repository.GetAsync(e => e.Id == id) != null)
            {
                await _repository.UpdateAsync(roles);
                return NoContent();
            }

            return NotFound();
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
                return NotFound();
            }
            await _repository.DeleteAsync(item);

            return NoContent();
        }
    }
}
