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
using Newtonsoft.Json.Linq;

namespace IceCreamBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountDetailController : ControllerBase
    {
        private readonly IRepositoryAccountDetail _IRepositoryAccountDetail;

        public AccountDetailController(IRepositoryAccountDetail iRepositoryAccountDetail)
        {
            _IRepositoryAccountDetail = iRepositoryAccountDetail;
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult> GetAccounts()
        {
            var value = new List<AccountDetailDTO>();
            var result = await _IRepositoryAccountDetail.GetAllAsync();
            result.ForEach(e => value.Add(new AccountDetailDTO
            {
                Id = e.Id,
                Avatar = e.Avatar,
                Email = e.Email,
                ExpirationDate = e.ExpirationDate,
                ExtensionDate = e.ExtensionDate,
                FullName = e.FullName,
                PhoneNumber = e.PhoneNumber,
                RoleID = e.RoleID,
            }));
            return Ok(value.ToList());
        }

        // GET: api/Accounts/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AccountDetailDTO>> GetAccount(string id)
        {
            var result = await _IRepositoryAccountDetail.GetAsync(e => e.Id == int.Parse(id));
            if (result != null)
            {
                return Ok(new AccountDetailDTO
                {
                    Id = result.Id,
                    Avatar = result.Avatar,
                    Email = result.Email,
                    ExpirationDate = result.ExpirationDate,
                    ExtensionDate = result.ExtensionDate,
                    FullName = result.FullName,
                    PhoneNumber = result.PhoneNumber,
                    RoleID = result.RoleID
                });
            }
            return NotFound();
        }

        //GET: api/Accounts/query
        [HttpGet("{query}")]
        public async Task<ActionResult<AccountDetailDTO>> GetAccounts(string query)
        {
            var value = new List<AccountDetailDTO>();
            var result = (await _IRepositoryAccountDetail.GetAllAsync(e => e.FullName.Contains(query)));
            result.ForEach(e => value.Add(new AccountDetailDTO
            {
                Id = e.Id,
                Avatar = e.Avatar,
                Email = e.Email,
                ExpirationDate = e.ExpirationDate,
                ExtensionDate = e.ExtensionDate,
                FullName = e.FullName,
                PhoneNumber = e.PhoneNumber,
                RoleID = e.RoleID
            }));
            return Ok(value);
        }


        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccounts(int id, AccountDetailDTO accounts)
        {
            if (id != accounts.Id)
            {
                return BadRequest();
            }

            var resultAccount = await _IRepositoryAccountDetail.GetAsync(e => e.Id == id);
            if (resultAccount != null)
            {
                await _IRepositoryAccountDetail.UpdateAsync(new AccountDetailDTO
                {
                    Id = accounts.Id,
                    RoleID = accounts.RoleID,
                    Avatar = accounts.Avatar,
                    Email = accounts.Email,
                    ExpirationDate = accounts.ExpirationDate,
                    ExtensionDate = accounts.ExtensionDate,
                    FullName = accounts.FullName,
                    PhoneNumber = accounts.PhoneNumber,
                });

                return NoContent();
            }
            return NotFound();
        }
    }
}
