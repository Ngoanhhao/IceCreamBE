using Azure.Identity;
using System.ComponentModel.DataAnnotations;

namespace IceCreamBE.DTO
{
    public class VouchersDTO
    {
        public int Id { get; set; }
        public string voucher { get; set; }
        public int adminID { get; set; }
        public int discount { get; set; } // percent
        public bool status { get; set; }
        public DateTime expiration_date { get; set; }
    }
}
