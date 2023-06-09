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
using IceCreamBE.DTO.PageList;
using Microsoft.AspNetCore.Mvc.Filters;
using IceCreamBE.Repository;
using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Authorization;

namespace IceCreamBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountDetailController : ControllerBase
    {
        private readonly IRepositoryAccountDetail _IRepositoryAccountDetail;
        private readonly IRepositoryAccounts _IRepositoryAccounts;
        private readonly IRepositoryRoles _IRepositoryRoles;
        private readonly IRepositoryFileService _IRepositoryFileService;

        public AccountDetailController(IRepositoryAccountDetail iRepositoryAccountDetail, IRepositoryAccounts repositoryAccounts, IRepositoryRoles iRepositoryRoles, IRepositoryFileService iRepositoryFileService)
        {
            _IRepositoryAccountDetail = iRepositoryAccountDetail;
            _IRepositoryAccounts = repositoryAccounts;
            _IRepositoryRoles = iRepositoryRoles;
            _IRepositoryFileService = iRepositoryFileService;
        }

        // GET: api/Accounts
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAccounts([FromQuery] PaginationFilter<AccountDetailDTO>? filter)
        {
            string url = $"{Request.Scheme}://{Request.Host}/api/image/";
            var value = new List<AccountDetailDTO>();
            var accountdetail = await _IRepositoryAccountDetail.GetAllAsync();
            var account = await _IRepositoryAccounts.GetAllAsync();
            var roles = await _IRepositoryRoles.GetAllAsync();
            var result = accountdetail
                .Join(roles,
                e => e.RoleID,
                q => q.Id,
                (e, q) => new { accountdetail = e, roles = q })
                .Join(account,
                e => e.accountdetail.Id,
                q => q.Id,
                (e, q) => new { accountdetail = e.accountdetail, roles = e.roles, account = q })
                .Select(e => new AccountDetailOutDTO
                {
                    Id = e.accountdetail.Id,
                    Avatar = _IRepositoryFileService.CheckImage(e.accountdetail.Avatar, "Images") ? url + e.accountdetail.Avatar : null,
                    Email = e.accountdetail.Email,
                    Expiration_date = e.accountdetail.ExpirationDate,
                    Extension_date = e.accountdetail.ExtensionDate,
                    Full_name = e.accountdetail.FullName,
                    UserName = e.account.Username,
                    Phone_number = e.accountdetail.PhoneNumber,
                    Address = e.accountdetail.Address,
                    Role = e.roles.Role,
                    Create_date = e.accountdetail.CreateDate
                }).ToList();

            var pageFilter = new PaginationFilter<AccountDetailOutDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(result);

            return Ok(new PagedResponse<List<AccountDetailOutDTO>>
            {
                Data = pagedData,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<AccountDetailOutDTO>>
                {
                    current_page = pagedData == null ? 0 : pageFilter.PageNumber,
                    Page_pize = pagedData == null ? 0 : pageFilter.PageSize,
                    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                    total_records = result.Count
                }
            });
        }

        // GET: api/Accounts/5
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AccountDetailOutDTO>> GetAccount(string id)
        {
            var result = await _IRepositoryAccountDetail.GetAsync(e => e.Id == int.Parse(id));
            if (result != null)
            {
                string url = $"{Request.Scheme}://{Request.Host}/api/image/";
                var account = await _IRepositoryAccounts.GetAsync(e => e.Id == result.Id);
                var roles = await _IRepositoryRoles.GetAsync(e => e.Id == result.RoleID);
                return Ok(
                    new Response<AccountDetailOutDTO>
                    {
                        Succeeded = true,
                        Data = new AccountDetailOutDTO
                        {
                            Id = result.Id,
                            Avatar = _IRepositoryFileService.CheckImage(result.Avatar, "Images") ? url + result.Avatar : null,
                            Email = result.Email,
                            Expiration_date = result.ExpirationDate,
                            Extension_date = result.ExtensionDate,
                            Full_name = result.FullName,
                            UserName = account.Username,
                            Phone_number = result.PhoneNumber,
                            Address = result.Address,
                            Role = roles.Role,
                            Create_date = result.CreateDate
                        }
                    }
                    );
            }
            return BadRequest(new Response<AccountDetailDTO>
            {
                Succeeded = false,
                Message = "not found"
            });
        }

        // GET: api/Accounts/5
        [HttpGet("/api/FoundAccount")]
        public async Task<ActionResult<AccountDetailOutDTO>> GetAccounts(string email)
        {
            var result = await _IRepositoryAccountDetail.GetAsync(e => e.Email.ToLower() == email.ToLower());
            if (result != null)
            {
                string url = $"{Request.Scheme}://{Request.Host}/api/image/";
                var account = await _IRepositoryAccounts.GetAsync(e => e.Id == result.Id);
                var roles = await _IRepositoryRoles.GetAsync(e => e.Id == result.RoleID);
                return Ok(
                    new Response<AccountDetailOutDTO>
                    {
                        Succeeded = true,
                        Data = new AccountDetailOutDTO
                        {
                            Id = result.Id,
                            Avatar = _IRepositoryFileService.CheckImage(result.Avatar, "Images") ? url + result.Avatar : null,
                            Email = result.Email,
                            Expiration_date = result.ExpirationDate,
                            Extension_date = result.ExtensionDate,
                            Full_name = result.FullName,
                            UserName = account.Username,
                            Phone_number = result.PhoneNumber,
                            Address = result.Address,
                            Role = roles.Role,
                            Create_date = result.CreateDate
                        }
                    }
                    );
            }
            return BadRequest(new Response<AccountDetailDTO>
            {
                Succeeded = false,
                Message = "not found"
            });
        }

        [HttpGet("/api/getme/{protectID}")]
        [Authorize]
        public async Task<ActionResult<AccountDetailDTO>> GetAccountss(Guid protectID)
        {
            var detail = await _IRepositoryAccountDetail.GetAsync(e => e.ProtectID == protectID);

            if (detail != null)
            {
                var user = await _IRepositoryAccounts.GetAsync(e => e.Id == detail.Id);
                var roles = await _IRepositoryRoles.GetAsync(e => e.Id == detail.RoleID);
                string url = $"{Request.Scheme}://{Request.Host}/api/image/";


                // check & update role
                if (detail.ExpirationDate < DateTime.Now)
                {
                    await _IRepositoryAccountDetail.UpdateRole(user.Id, 3);
                }

                return Ok(new Response<AccountDetailOutDTO>
                {
                    Data = new AccountDetailOutDTO
                    {
                        Id = user.Id,
                        UserName = user.Username,
                        Full_name = detail.FullName,
                        ProtectID = detail.ProtectID,
                        Email = detail.Email,
                        Phone_number = detail.PhoneNumber,
                        Avatar = _IRepositoryFileService.CheckImage(detail.Avatar, "Images") ? url + detail.Avatar : null,
                        Role = roles.Role,
                        Address = detail.Address,
                        Expiration_date = detail.ExpirationDate,
                        Extension_date = detail.ExtensionDate,
                        Create_date = detail.CreateDate,
                    },
                    Succeeded = true
                });
            }

            return BadRequest(new Response<AccountDetailDTO>
            {
                Succeeded = false,
                Message = "not found"
            });
        }

        //GET: api/Accounts/query
        [HttpGet("/api/search/user")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<AccountDetailDTO>>> GetAccounts(string? query, [FromQuery] PaginationFilter<BrandsDTO>? filter)
        {
            var value = new List<AccountDetailDTO>();
            var result = (await _IRepositoryAccountDetail.GetAllAsync(e => e.FullName.ToLower().Contains(query != null ? query.ToLower() : "")));
            string url = $"{Request.Scheme}://{Request.Host}/api/image/";
            result.ForEach(e => value.Add(new AccountDetailDTO
            {
                Id = e.Id,
                Avatar = _IRepositoryFileService.CheckImage(e.Avatar, "Images") ? url + e.Avatar : null,
                Email = e.Email,
                Expiration_date = e.ExpirationDate,
                Extension_date = e.ExtensionDate,
                Full_name = e.FullName,
                Address = e.Address,
                Phone_number = e.PhoneNumber,
                RoleID = e.RoleID
            }));

            if (result.Count == 0)
            {
                return BadRequest(new Response<List<AccountDetailDTO>>
                {
                    Succeeded = false,
                    Message = "not found"
                });
            }

            var pageFilter = new PaginationFilter<BrandsDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(value);

            return Ok(new PagedResponse<List<AccountDetailDTO>>
            {
                Data = pagedData,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<AccountDetailDTO>>
                {
                    current_page = pagedData == null ? 0 : pageFilter.PageNumber,
                    Page_pize = pagedData == null ? 0 : pageFilter.PageSize,
                    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                    total_records = result.Count
                }
            });
        }


        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutAccounts(int id, AccountDetailDTO accounts)
        {
            if (id != accounts.Id)
            {
                return BadRequest(new Response<List<AccountDetailDTO>>
                {
                    Succeeded = false,
                    Message = "value incorrect"
                });
            }

            var resultAccount = await _IRepositoryAccountDetail.GetAsync(e => e.Id == id);
            if (resultAccount != null)
            {
                await _IRepositoryAccountDetail.UpdateAsync(new AccountDetailDTO
                {
                    Id = accounts.Id,
                    Avatar = accounts.Avatar,
                    Address = accounts.Address,
                    Full_name = accounts.Full_name,
                    Phone_number = accounts.Phone_number,
                });

                return NoContent();
            }
            return BadRequest(new Response<List<AccountDetailDTO>>
            {
                Succeeded = false,
                Message = "not found"
            });
        }
    }
}
