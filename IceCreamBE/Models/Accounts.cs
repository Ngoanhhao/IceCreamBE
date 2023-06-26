using System.ComponentModel.DataAnnotations;

namespace IceCreamBE.Models
{
    public class Accounts
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public virtual AccountDetail AccountDetail { get; set; }
        //public virtual ICollection<Feedback> Feedback { get; set; }
        public virtual ICollection<Bill> Bill { get; set; }
        public virtual ICollection<Vouchers> vouchers { get; set; }
        public virtual ICollection<RefreshToken> RefreshToken { get; set; }
    }
}
