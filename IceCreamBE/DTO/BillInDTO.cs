namespace IceCreamBE.DTO
{
    public class BillInDTO
    {
        public int Id { get; set; }
        public bool status { get; set; }
        public int accountID { get; set; }
        public int? voucherID { get; set; }
    }
}
