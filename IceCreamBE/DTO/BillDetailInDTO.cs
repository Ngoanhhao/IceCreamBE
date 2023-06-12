using System.ComponentModel.DataAnnotations;

namespace IceCreamBE.DTO
{
    public class BillDetailInDTO
    {
        public int Id { get; set; }
        public int productID { get; set; }
        public int quantity { get; set; }
    }
}
