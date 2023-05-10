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
using Newtonsoft.Json.Linq;
using IceCreamBE.DTO.PageList;
using Microsoft.AspNetCore.Mvc.Filters;

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
        public async Task<ActionResult> GetAccounts([FromQuery] PaginationFilter<BrandsDTO>? filter)
        {
            var value = new List<AccountDetailDTO>();
            var result = await _IRepositoryAccountDetail.GetAllAsync();
            result.ForEach(e => value.Add(new AccountDetailDTO
            {
                Id = e.Id,
                Avatar = e.Avatar,
                Email = e.Email,
                Expiration_date = e.ExpirationDate,
                Extension_date = e.ExtensionDate,
                Full_name = e.FullName,
                Phone_number = e.PhoneNumber,
                RoleID = e.RoleID,
            }));

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

        // GET: api/Accounts/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AccountDetailDTO>> GetAccount(string id)
        {
            var result = await _IRepositoryAccountDetail.GetAsync(e => e.Id == int.Parse(id));
            if (result != null)
            {
                return Ok(
                    new Response<AccountDetailDTO>
                    {
                        Succeeded = true,
                        Data = new AccountDetailDTO
                        {
                            Id = result.Id,
                            Avatar = result.Avatar,
                            Email = result.Email,
                            Expiration_date = result.ExpirationDate,
                            Extension_date = result.ExtensionDate,
                            Full_name = result.FullName,
                            Phone_number = result.PhoneNumber,
                            RoleID = result.RoleID
                        }
                    }
                    );
            }
            return NotFound(new Response<AccountDetailDTO>
            {
                Succeeded = false,
                Message = "not found"
            });
        }

        //GET: api/Accounts/query
        [HttpGet("{query}")]
        public async Task<ActionResult<AccountDetailDTO>> GetAccounts(string query, [FromQuery] PaginationFilter<BrandsDTO>? filter)
        {
            var value = new List<AccountDetailDTO>();
            var result = (await _IRepositoryAccountDetail.GetAllAsync(e => e.FullName.Contains(query)));
            result.ForEach(e => value.Add(new AccountDetailDTO
            {
                Id = e.Id,
                Avatar = e.Avatar,
                Email = e.Email,
                Expiration_date = e.ExpirationDate,
                Extension_date = e.ExtensionDate,
                Full_name = e.FullName,
                Phone_number = e.PhoneNumber,
                RoleID = e.RoleID
            }));

            if (result.Count == 0)
            {
                return NotFound(new Response<List<AccountDetailDTO>>
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
                    Expiration_date = accounts.Expiration_date,
                    Extension_date = accounts.Extension_date,
                    Full_name = accounts.Full_name,
                    Phone_number = accounts.Phone_number,
                });

                return NoContent();
            }
            return NotFound(new Response<List<AccountDetailDTO>>
            {
                Succeeded = false,
                Message = "not found"
            });
        }
    }
}
