using System.ComponentModel.DataAnnotations;

namespace IceCreamBE.DTO
{
    public class AccountDetailDTO
    {
        public int Id { get; set; }
        public int? RoleID { get; set; }
        public string Email { get; set; }
        public string Full_name { get; set; }
        public string? Avatar { get; set; }
        public string Phone_number { get; set; }
        public string Address { get; set; }
        public DateTime? Create_date { get; set; }
        public DateTime? Extension_date { get; set; }
        public DateTime? Expiration_date { get; set; }
    }
}
