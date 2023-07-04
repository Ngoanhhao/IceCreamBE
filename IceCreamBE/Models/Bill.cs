namespace IceCreamBE.Models
{
    public class Bill
    {
        public int Id { get; set; }
        public int AccountID { get; set; }
        public Accounts Account { get; set; }
        public virtual ICollection<BillDetail> BillDetail { get; set; }
        public double SubTotal { get; set; }
        public int? Discount { get; set; }
        public double Total { get; set; }
        public string Status { get; set; }
        public DateTime OrderTime { get; set; }
        public int? VoucherID { get; set; }
        public virtual Vouchers Vouchers { get; set; }
    }
}
