using Azure.Identity;
using System.ComponentModel.DataAnnotations;

namespace IceCreamBE.DTO
{
    public class VouchersDTO
    {
        public int Id { get; set; }
        public string Voucher { get; set; }
        public int AdminID { get; set; }
        public int Discount { get; set; } // percent
        public bool Status { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
