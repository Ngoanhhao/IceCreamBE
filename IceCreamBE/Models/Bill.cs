namespace IceCreamBE.Models
{
    public class Bill
    {
        public int Id { get; set; }
        public int AccountID { get; set; }
        public Accounts Account { get; set; }
        public int BillDetailID { get; set; }
        public virtual ICollection<BillDetail> BillDetail { get; set; }
        //public int VoucherID { get; set; }
        //public virtual Vouchers Voucher { get; set; }
        public double Total { get; set; }
        public double Status { get; set; }
        public DateTime OrderTime { get; set; }
        public int? VoucherID { get; set; }
        public Vouchers Vouchers { get; set; }
    }
}
