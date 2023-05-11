namespace IceCreamBE.DTO
{
    public class BillOutDTO
    {
        public int Id { get; set; }
        public string full_name { get; set; }
        public BillDetailOutDTO bill_detail{ get; set; }
        public double total { get; set; }
        public double status { get; set; }
        public DateTime order_Time { get; set; }
        public string? voucher { get; set; }
    }
}
