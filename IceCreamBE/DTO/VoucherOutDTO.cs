namespace IceCreamBE.DTO
{
    public class VoucherOutDTO
    {
        public int Id { get; set; }
        public string user_name { get; set; }
        public string voucher { get; set; }
        public int discount_percent { get; set; } // percent
        public bool status { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
