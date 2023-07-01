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
using IceCreamBE.Modules;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Policy;

namespace IceCreamBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IRepositoryAccounts _RepositoryAccounts;
        private readonly IRepositoryAccountDetail _RepositoryAccountDetail;
        private readonly IRepositoryRoles _IRepositoryRoles;
        private readonly IHandleResponseCode _HandleResponseCode;
        private readonly IRepositoryFileService _IRepositoryFileService;
        private readonly IMailHandle _IMailHandle;

        public AccountsController(
            IRepositoryAccounts RepositoryAccounts,
            IRepositoryAccountDetail repositoryAccountDetail,
            IRepositoryRoles repositoryRoles, IHandleResponseCode handleResponseCode,
            IRepositoryFileService iRepositoryFileService,
            IMailHandle iMailHandle
            )
        {
            _RepositoryAccounts = RepositoryAccounts;
            _RepositoryAccountDetail = repositoryAccountDetail;
            _IRepositoryRoles = repositoryRoles;
            _HandleResponseCode = handleResponseCode;
            _IRepositoryFileService = iRepositoryFileService;
            _IMailHandle = iMailHandle;
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("/api/ResetPassword/{userId}")]
        public async Task<IActionResult> ResetPassword(int userId)
        {


            var result = await _RepositoryAccountDetail.GetAsync(e => e.Id == userId);
            if (result == null)
            {
                return BadRequest(new Response<List<AccountDetailDTO>>
                {
                    Succeeded = false,
                    Message = "not found"
                });
            }
            var random = Coupon.CouponGenarate(10);

            await _RepositoryAccounts.UpdateAsync(new AccountDTO { Password = MD5Generator.MD5Encryption(random), Id = result.Id });

            _IMailHandle.send("Reset Password", "<h3 style='font-weight: 100; color: black'>" +
                "We received a request to reset your password. " +
                "Don’t worry, we are here to help you. <br>" +
                "Here your new password, please change new password if you see this mail </h3>" +
                "<h2 style='color: black'>New password: " + random + "</h2>" +
                "<button style='padding: 10px 50px;font-size: 1.5rem;border-radius: 50px;color: white;background: #ed7399;border: 0;'>Login</button>",
                result.Email);

            return NoContent();
        }


        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("/api/ChangePassword/{userId}")]
        public async Task<IActionResult> ChangePassword(int userId, string old_password, string new_password)
        {


            var result = await _RepositoryAccounts.GetAsync(e => e.Id == userId);
            if (result == null)
            {
                return BadRequest(new Response<List<AccountDetailDTO>>
                {
                    Succeeded = false,
                    Message = "not found"
                });
            }
            if (result.Password != MD5Generator.MD5Encryption(old_password))
            {
                return BadRequest(new Response<List<AccountDetailDTO>>
                {
                    Succeeded = false,
                    Message = "old password not correct please try again"
                });
            }

            await _RepositoryAccounts.UpdateAsync(new AccountDTO { Password = MD5Generator.MD5Encryption(new_password), Id = result.Id });


            return NoContent();
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("/api/ForgotPassword/{userId}")]
        public async Task<IActionResult> ForgotPassword(int userId, string new_password)
        {


            var result = await _RepositoryAccounts.GetAsync(e => e.Id == userId);
            if (result == null)
            {
                return BadRequest(new Response<List<AccountDetailDTO>>
                {
                    Succeeded = false,
                    Message = "not found"
                });
            }

            await _RepositoryAccounts.UpdateAsync(new AccountDTO { Password = MD5Generator.MD5Encryption(new_password), Id = result.Id });


            return NoContent();
        }

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/api/register")]
        public async Task<ActionResult<Accounts>> PostAccounts(RegisterDTO entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<List<AccountDetailDTO>>
                {
                    Succeeded = false,
                    Message = "value incorrect"
                });
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

            var email = await _RepositoryAccountDetail.GetAsync(e => e.Email.ToLower() == entity.email.ToLower());
            if (email != null)
            {
                return BadRequest(new Response<List<AccountDetailDTO>>
                {
                    Succeeded = false,
                    Message = "email is valid"
                });
            }


            await _RepositoryAccounts.CreateAsync(new Accounts
            {
                Username = entity.username,
                Password = MD5Generator.MD5Encryption(entity.password)
            });
            var user = await _RepositoryAccounts.GetAsync(e => e.Username.ToLower() == entity.username.ToLower());

            await _RepositoryAccountDetail.CreateAsync(new AccountDetail
            {
                Id = user.Id,
                ProtectID = Guid.NewGuid(),
                Avatar = null,
                Email = entity.email,
                FullName = entity.username,
                PhoneNumber = null,
                RoleID = 3,
                CreateDate = DateTime.Now,
            });

            var userdetail = await _RepositoryAccountDetail.GetAsync(e => e.Id == user.Id);

            return Ok(new Response<AccountDetailDTO>
            {
                Succeeded = true,
                Data = new AccountDetailDTO
                {
                    Id = user.Id,
                    Avatar = null,
                    Full_name = userdetail.FullName,
                    Phone_number = userdetail.PhoneNumber,
                    Address = userdetail.Address,

                    RoleID = userdetail.RoleID,
                    Expiration_date = null,
                    Extension_date = null,
                    Email = userdetail.Email,
                    Create_date = userdetail.CreateDate
                }
            });
        }

        [HttpPut("/api/updateaccount")]
        public async Task<ActionResult<Accounts>> UpdateInfo(AccountDetailDTO entity)
        {
            var userdetail = await _RepositoryAccountDetail.GetAsync(e => e.Id == entity.Id);

            if (userdetail == null)
            {
                return NotFound(new Response<List<AccountDetailDTO>>
                {
                    Succeeded = false,
                    Message = "user incorrect"
                });
            }

            await _RepositoryAccountDetail.UpdateAsync(new AccountDetailDTO
            {
                Id = userdetail.Id,
                Avatar = entity.Avatar,
                Full_name = entity.Full_name,
                Phone_number = entity.Phone_number,
                Address = entity.Address,
            });

            return NoContent();
        }

        [HttpPut("/api/updatepremium")]
        public async Task<ActionResult<Accounts>> UpdatePremium(int userID, int month)
        {
            var userdetail = await _RepositoryAccountDetail.GetAsync(e => e.Id == userID);

            if (userdetail == null)
            {
                return NotFound(new Response<List<AccountDetailDTO>>
                {
                    Succeeded = false,
                    Message = "user incorrect"
                });
            }

            await _RepositoryAccountDetail.UpdatePremium(userID, month);

            return NoContent();
        }
    }
}
