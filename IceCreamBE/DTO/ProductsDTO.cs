using System.ComponentModel.DataAnnotations;

namespace IceCreamBE.DTO
{
    public class ProductsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BrandID { get; set; }
        public double Cost { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }
        public int Discount { get; set; } // percent
        public int Total { get; set; }
        public bool Status { get; set; }
    }
}
