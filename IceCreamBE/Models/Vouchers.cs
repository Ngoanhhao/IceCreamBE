using Azure.Identity;
using System.ComponentModel.DataAnnotations;

namespace IceCreamBE.Models
{
    public class Vouchers
    {
        public int Id { get; set; }
        public string Voucher { get; set; }
        public int AdminID { get; set; }
        public virtual Accounts Admin { get; set; }
        public int Discount { get; set; } // percent
        public bool Status { get; set; }
        public DateTime ExpirationDate { get; set; }
        public ICollection<Bill> Bill { get; set; }
    }
}
