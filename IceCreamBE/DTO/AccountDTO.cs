using System.ComponentModel.DataAnnotations;

namespace IceCreamBE.DTO
{
    public class AccountDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
