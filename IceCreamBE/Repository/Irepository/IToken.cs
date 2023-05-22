using IceCreamBE.DTO;
using IceCreamBE.Models;

namespace IceCreamBE.Repository.Irepository
{
    public interface IToken
    {
        public string getToken(AccountDetailOutDTO accounts);
        public string getRefreshToken();
    }
}
