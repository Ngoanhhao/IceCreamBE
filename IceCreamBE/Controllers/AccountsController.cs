﻿using System;
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
                return NotFound(new Response<List<AccountDetailDTO>>
                {
                    Succeeded = false,
                    Message = "not found"
                });
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

            var result = await _RepositoryAccounts.GetAsync(e => e.Username.ToLower() == entity.username.ToLower());
            if (result != null)
            {
                return BadRequest(new Response<List<AccountDetailDTO>>
                {
                    Succeeded = false,
                    Message = "username is valid"
                });
            }

            await _RepositoryAccounts.CreateAsync(new Accounts
            {
                Username = entity.username,
                Password = entity.password
            });

            var result2 = await _RepositoryAccounts.GetAsync(e => e.Username == entity.username && e.Password == entity.password);

            await _RepositoryAccountDetail.CreateAsync(new AccountDetail
            {
                Id = result2.Id,
                Avatar = entity.avatar,
                Email = entity.email,
                ExpirationDate = entity.expiration_date,
                ExtensionDate = entity.extension_date,
                FullName = entity.full_name,
                PhoneNumber = entity.phone_number,
                RoleID = entity.roleID,
            });

            return Ok(new Response<AccountDetailDTO>
            {
                Succeeded = true,
                Data = new AccountDetailDTO
                {
                    Id = result2.Id,
                    Avatar = entity.avatar,
                    Email = entity.email,
                    Expiration_date = entity.expiration_date,
                    Extension_date = entity.extension_date,
                    Full_name = entity.full_name,
                    Phone_number = entity.phone_number,
                    RoleID = entity.roleID,
                }
            });
        }
    }
}
