namespace IceCreamBE.DTO
{
    public class BillDTO
    {
        public int Id { get; set; }
        public int AccountID { get; set; }
        public int BillDetailID { get; set; }
        public double Total { get; set; }
        public double Status { get; set; }
        public DateTime OrderTime { get; set; }
        public int? VoucherID { get; set; }
    }
}
