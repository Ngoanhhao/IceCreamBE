using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IceCreamBE.Data;
using IceCreamBE.Models;
using IceCreamBE.Repository;
using IceCreamBE.DTO;
using IceCreamBE.Repository.Irepository;

namespace IceCreamBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IRepositoryAccounts _RepositoryAccounts;
        private readonly IRepositoryAccountDetail _RepositoryAccountDetail;

        public AccountsController(IRepositoryAccounts RepositoryAccounts, IRepositoryAccountDetail repositoryAccountDetail)
        {
            _RepositoryAccounts = RepositoryAccounts;
            _RepositoryAccountDetail = repositoryAccountDetail;
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccounts(int id, AccountDTO accounts)
        {
            if (id != accounts.Id)
            {
                return BadRequest();
            }

            var result = await _RepositoryAccounts.GetAsync(e => e.Id == id);
            if (result == null)
            {
                return NotFound();
            }

            await _RepositoryAccounts.UpdateAsync(accounts);

            return NoContent();
        }

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Accounts>> PostAccounts(RegisterDTO entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _RepositoryAccounts.GetAsync(e => e.Username.ToLower() == entity.Username.ToLower());
            if (result != null)
            {
                //return Ok(new Response
                //{
                //    Message = "Username is valid",
                //    Success = false,
                //});
                return BadRequest();
            }

            await _RepositoryAccounts.CreateAsync(new Accounts
            {
                Username = entity.Username,
                Password = entity.Password
            });

            var result2 = await _RepositoryAccounts.GetAsync(e => e.Username == entity.Username && e.Password == entity.Password);

            await _RepositoryAccountDetail.CreateAsync(new AccountDetail
            {
                Id = result2.Id,
                Avatar = entity.Avatar,
                Email = entity.Email,
                ExpirationDate = entity.ExpirationDate,
                ExtensionDate = entity.ExtensionDate,
                FullName = entity.FullName,
                PhoneNumber = entity.PhoneNumber,
                RoleID = entity.RoleID,
            });

            //return Ok(new Response
            //{
            //    Message = "Successed",
            //    Success = true,
            //    Result = new AccountDetail
            //    {
            //        Id = result2.Id,
            //        Avatar = entity.Avatar,
            //        Email = entity.Email,
            //        ExpirationDate = entity.ExpirationDate,
            //        ExtensionDate = entity.ExtensionDate,
            //        FullName = entity.FullName,
            //        PhoneNumber = entity.PhoneNumber,
            //        RoleID = entity.RoleID,
            //    }
            //});
            return Ok(new AccountDetail
            {
                Id = result2.Id,
                Avatar = entity.Avatar,
                Email = entity.Email,
                ExpirationDate = entity.ExpirationDate,
                ExtensionDate = entity.ExtensionDate,
                FullName = entity.FullName,
                PhoneNumber = entity.PhoneNumber,
                RoleID = entity.RoleID,
            });
        }
    }
}
