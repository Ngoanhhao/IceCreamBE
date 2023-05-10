using System.ComponentModel.DataAnnotations;

namespace IceCreamBE.DTO
{
    public class ProductsDTO
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int brandID { get; set; }
        public double cost { get; set; }
        public double price { get; set; }
        public string description { get; set; }
        public string img { get; set; }
        public int discount { get; set; } // percent
        public int total { get; set; }
        public bool status { get; set; }
    }
}
