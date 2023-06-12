namespace IceCreamBE.DTO
{
    public class VoucherInDTO
    {
        public int Id { get; set; }
        public int adminID { get; set; }
        public int discount_percent { get; set; } // percent
        public bool status { get; set; }
        public int? hourExpiration { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
