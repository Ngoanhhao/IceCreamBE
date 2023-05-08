using System.ComponentModel.DataAnnotations;

namespace IceCreamBE.DTO
{
    public class BillDetail
    {
        public int Id { get; set; }
        public int BillID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
    }
}
