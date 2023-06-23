using IceCreamBE.DTO;
using IceCreamBE.Migrations;
using IceCreamBE.Models;
using IceCreamBE.Modules;
using IceCreamBE.Repository.Irepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Policy;
using System.Text;

namespace IceCreamBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IRepositoryAccounts _RepositoryAccounts;
        private readonly IRepositoryAccountDetail _IRepositoryAccountDetail;
        private readonly IRepositoryRoles _IRepositoryRoles;
        private readonly IToken _Token;
        private readonly IRepositoryRefreshtoken _IRepositoryRefreshtoken;
        private readonly IConfiguration _Configuration;
        private readonly IRepositoryFileService _IRepositoryFileService;


        public LoginController(IRepositoryAccounts RepositoryAccounts, IRepositoryAccountDetail IRepositoryAccountDetail, IRepositoryRoles IRepositoryRoles, IToken token, IRepositoryRefreshtoken iRepositoryRefreshtoken, IConfiguration configuration, IRepositoryFileService iRepositoryFileService)
        {
            _RepositoryAccounts = RepositoryAccounts;
            _IRepositoryAccountDetail = IRepositoryAccountDetail;
            _IRepositoryRoles = IRepositoryRoles;
            _Token = token;
            _IRepositoryRefreshtoken = iRepositoryRefreshtoken;
            _Configuration = configuration;
            _IRepositoryFileService = iRepositoryFileService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO user)
        {
            var result = await _RepositoryAccounts.GetAsync(e => e.Username == user.username && e.Password == MD5Generator.MD5Encryption(user.password));
            if (result == null)
            {
                return Unauthorized(new Response<AccountDetailDTO> { Message = "username or password incorrect", Succeeded = false });
            }

            var detail = await _IRepositoryAccountDetail.GetAsync(e => e.Id == result.Id);
            var roles = await _IRepositoryRoles.GetAsync(e => e.Id == detail.RoleID);
            string url = $"{Request.Scheme}://{Request.Host}/api/image/";
            // account info
            var data = new AccountDetailOutDTO
            {
                Id = result.Id,
                UserName = user.username,
                Full_name = detail.FullName,
                Email = detail.Email,
                Phone_number = detail.PhoneNumber,
                Avatar = _IRepositoryFileService.CheckImage(detail.Avatar, "Images") ? url + detail.Avatar : null,
                Role = roles.Role,
                Expiration_date = detail.ExpirationDate,
                Extension_date = detail.ExtensionDate
            };

            // token
            var token = _Token.getToken(data);
            // create refresh token
            var refreshToken = _Token.getRefreshToken();
            await _IRepositoryRefreshtoken.CreateAsync(new Models.RefreshToken
            {
                createDate = DateTime.UtcNow,
                expirationDate = DateTime.UtcNow.AddDays(7),
                isUsed = false,
                accessToken = token,
                refreshToken = refreshToken,
                userId = data.Id
            });

            return Ok(new Response<AccountDetailOutDTO>
            {
                Data = data,
                Token = new TokenOutDTO { Token = token, Refresh_Token = refreshToken },
                Succeeded = true
            });
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> getToken(RefreshTokenDTO Token)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_Configuration["Authentication:key"]);
            var tokenValidateParam = new TokenValidationParameters
            {
                //tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                //ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false //ko kiểm tra token hết hạn
            };
            try
            {
                //check 1: AccessToken valid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(Token.AccessToken, tokenValidateParam, out var validatedToken);

                //check 2: Check alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)//false
                    {
                        return Ok(new Response<TokenOutDTO>
                        {
                            Succeeded = false,
                            Message = "Invalid token"
                        });
                    }
                }

                //check 3: Check accessToken expire?
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                if (expireDate > DateTime.UtcNow)
                {
                    return Ok(new Response<TokenOutDTO>
                    {
                        Succeeded = false,
                        Message = "Access token has not yet expired"
                    });
                }

                //check 4: Check refreshtoken exist in DB
                var storedToken = await _IRepositoryRefreshtoken.GetAsync(x => x.refreshToken == Token.RefreshToken);
                if (storedToken == null)
                {
                    return Ok(new Response<TokenOutDTO>
                    {
                        Succeeded = false,
                        Message = "Refresh token does not exist"
                    });
                }

                //check 5: check refreshToken is used/revoked?
                if (storedToken.isUsed)
                {
                    return Ok(new Response<TokenOutDTO>
                    {
                        Succeeded = false,
                        Message = "Refresh token has been used"
                    });
                }

                //check 6: AccessToken id == JwtId in RefreshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == "ID").Value;
                if (storedToken.userId != int.Parse(jti))
                {
                    return Ok(new Response<TokenOutDTO>
                    {
                        Succeeded = false,
                        Message = "Token doesn't match"
                    });
                }

                //Update token is used
                await _IRepositoryRefreshtoken.UpdateAsync(storedToken);

                //create new token
                var user = await _RepositoryAccounts.GetAsync(nd => nd.Id == storedToken.userId);
                var userDetail = await _IRepositoryAccountDetail.GetAsync(e => e.Id == storedToken.userId);
                var role = await _IRepositoryRoles.GetAsync(e => e.Id == userDetail.RoleID);
                string url = $"{Request.Scheme}://{Request.Host}/api/image/";
                var data = new AccountDetailOutDTO
                {
                    Id = user.Id,
                    UserName = user.Username,
                    Full_name = userDetail.FullName,
                    Email = userDetail.Email,
                    Phone_number = userDetail.PhoneNumber,
                    Avatar = _IRepositoryFileService.CheckImage(userDetail.Avatar, "Images") ? url + userDetail.Avatar : null,
                    Role = role.Role,
                    Expiration_date = userDetail.ExpirationDate,
                    Extension_date = userDetail.ExtensionDate
                };
                var token = _Token.getToken(data);

                var refreshToken = _Token.getRefreshToken();

                await _IRepositoryRefreshtoken.CreateAsync(new Models.RefreshToken
                {
                    createDate = DateTime.UtcNow,
                    expirationDate = DateTime.UtcNow.AddDays(7),
                    isUsed = false,
                    accessToken = token,
                    refreshToken = refreshToken,
                    userId = data.Id
                });


                return Ok(new Response<string>
                {
                    Succeeded = true,
                    Message = "Renew token success",
                    Token = new TokenOutDTO { Token = token },
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<TokenOutDTO>
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                });
            }
        }
        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(utcExpireDate);
            DateTime dateTime = dateTimeOffset.LocalDateTime;

            return dateTime;
        }
    }
}
