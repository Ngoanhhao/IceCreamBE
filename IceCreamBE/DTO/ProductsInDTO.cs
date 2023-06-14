using System.ComponentModel.DataAnnotations;

namespace IceCreamBE.DTO
{
    public class ProductsInDTO
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int brandID { get; set; }
        public double cost { get; set; }
        public double price { get; set; }
        public int? discount_percent { get; set; } // percent
        public string? description { get; set; }
        public string? img { get; set; }
        public bool status { get; set; }
    }
}
