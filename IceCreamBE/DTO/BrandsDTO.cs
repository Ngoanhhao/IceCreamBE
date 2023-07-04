using System.ComponentModel.DataAnnotations;

namespace IceCreamBE.DTO
{
    public class BrandsDTO
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int? product_count { get; set; }
    }
}
