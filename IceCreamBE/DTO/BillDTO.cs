namespace IceCreamBE.DTO
{
    public class BillDTO
    {
        public int Id { get; set; }
        public int accountID { get; set; }
        public int bill_DetailID { get; set; }
        public double total { get; set; }
        public double status { get; set; }
        public DateTime order_Time { get; set; }
        public int? voucherID { get; set; }
    }
}
