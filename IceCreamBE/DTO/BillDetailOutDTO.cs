using System.ComponentModel.DataAnnotations;

namespace IceCreamBE.DTO
{
    public class BillDetailOutDTO
    {
        public int Id { get; set; }
        public int billID { get; set; }
        public string product_name { get; set; }
        public string brand_name { get; set; }
        public string img { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
        public double total { get; set; }
    }
}
