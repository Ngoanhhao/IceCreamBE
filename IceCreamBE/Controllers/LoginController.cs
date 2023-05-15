using IceCreamBE.DTO;
using IceCreamBE.Repository.Irepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IceCreamBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IRepositoryAccounts _RepositoryAccounts;
        private readonly IRepositoryAccountDetail _IRepositoryAccountDetail;
        private readonly IRepositoryRoles _IRepositoryRoles;

        public LoginController(IRepositoryAccounts RepositoryAccounts, IRepositoryAccountDetail IRepositoryAccountDetail, IRepositoryRoles IRepositoryRoles)
        {
            _RepositoryAccounts = RepositoryAccounts;
            _IRepositoryAccountDetail = IRepositoryAccountDetail;
            _IRepositoryRoles = IRepositoryRoles;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string username, string password)
        {
            var result = await _RepositoryAccounts.GetAsync(e => e.Username == username && e.Password == password);
            if (result == null)
            {
                return NotFound(new Response<AccountDetailDTO> { Message = "username or password incorrect", Succeeded = false });
            }

            var detail = await _IRepositoryAccountDetail.GetAsync(e => e.Id == result.Id);
            var roles = await _IRepositoryRoles.GetAsync(e => e.Id == detail.RoleID);
            return Ok(new Response<AccountDetailOutDTO>
            {
                Data = new AccountDetailOutDTO
                {
                    Id = result.Id,
                    UserName = username,
                    Full_name = detail.FullName,
                    Email = detail.Email,
                    Phone_number = detail.PhoneNumber,
                    Avatar = detail.Avatar,
                    Role = roles.Role,
                    Expiration_date = detail.ExpirationDate,
                    Extension_date = detail.ExtensionDate
                },
                Succeeded = true
            });
        }
    }
}
