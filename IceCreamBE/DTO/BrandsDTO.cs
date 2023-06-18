using System.ComponentModel.DataAnnotations;

namespace IceCreamBE.DTO
{
    public class BrandsDTO
    {
        public int Id { get; set; }
        public string brand_name { get; set; }
        public int? product_count { get; set; }
    }
}
