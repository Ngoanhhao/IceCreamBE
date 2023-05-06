using System.ComponentModel.DataAnnotations;

namespace IceCreamBE.Models
{
    public class Products
    {
        public int Id { get; set; }
        public Storage Storage { get; set; }
        public string Name { get; set; }
        public int BrandID { get; set; }
        public virtual Brands Brand { get; set; }
        public double Cost { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }
        public int Discount { get; set; } // percent
        public int Total { get; set; }
        public bool Status { get; set; }
        public ICollection<BillDetail> Details { get; set; }
        public ICollection<Recipe> Recipe { get; set; }
    }
}
