using System.ComponentModel.DataAnnotations;

namespace IceCreamBE.Models
{
    public class BillDetail
    {
        public int Id { get; set; }
        public int BillID { get; set; }
        public virtual Bill Bill { get; set; }
        public int ProductID { get; set; }
        public double Price { get; set; }
        public virtual Products Product { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
    }
}
