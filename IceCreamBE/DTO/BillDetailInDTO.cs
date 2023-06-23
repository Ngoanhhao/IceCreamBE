using System.ComponentModel.DataAnnotations;

namespace IceCreamBE.DTO
{
    public class BillDetailInDTO
    {
        public int? id { get; set; }
        public int user_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
    }
}
