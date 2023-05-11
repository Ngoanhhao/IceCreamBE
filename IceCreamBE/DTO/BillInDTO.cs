namespace IceCreamBE.DTO
{
    public class BillInDTO
    {
        public int Id { get; set; }
        public int accountID { get; set; }
        public List<BillDetailOutDTO> bill_detail { get; set; }
        public double total { get; set; }
        public double status { get; set; }
        public DateTime order_Time { get; set; }
        public int? voucherID { get; set; }
    }
}
