using IceCreamBE.Models;
using IceCreamBE.Repository.Irepository;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using IceCreamBE.DTO;
using System.Security.Cryptography;

namespace IceCreamBE.Repository
{
    public class Token : IToken
    {
        IConfiguration _configuration;

        public Token()
        {
        }

        public Token(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string getRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public string getToken(AccountDetailOutDTO accounts)
        {
            var JwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var tokentDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("ID", accounts.Id.ToString()),
                    new Claim("User", accounts.UserName.ToString()),
                    new Claim(ClaimTypes.Role, accounts.Role)
                }),

                Expires = DateTime.UtcNow.AddHours(2),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:key"])),
                    SecurityAlgorithms.HmacSha512Signature
                )
            };

            var token = JwtSecurityTokenHandler.CreateToken(tokentDescription);

            return JwtSecurityTokenHandler.WriteToken(token);
        }


    }
}
