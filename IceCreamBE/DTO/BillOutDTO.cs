namespace IceCreamBE.DTO
{
    public class BillOutDTO
    {
        public int Id { get; set; }
        public string full_name { get; set; }
        public string email { get; set; }
        public string? address { get; set; }
        public string phone_number { get; set; }
        public List<BillDetailOutDTO> products { get; set; }
        public double price { get; set; }
        public double sub_total { get; set; }
        public double total { get; set; }
        public string status { get; set; }
        public string? voucher { get; set; }
        public int? discount { get; set; }
        public DateTime order_Time { get; set; }
    }
}
