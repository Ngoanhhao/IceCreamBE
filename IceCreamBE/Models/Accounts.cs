using System.ComponentModel.DataAnnotations;

namespace IceCreamBE.Models
{
    public class Accounts
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public AccountDetail AccountDetail { get; set; }
        public int RoleID { get; set; }
        public Roles Role { get; set; }
        public ICollection<Feedback> Feedback { get; set; }
        public ICollection<Bill> Bill { get; set; }
        public ICollection<Vouchers> vouchers { get; set; }
    }
}
