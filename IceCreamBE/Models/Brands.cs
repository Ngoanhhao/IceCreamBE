using System.ComponentModel.DataAnnotations;

namespace IceCreamBE.Models
{
    public class Brands
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string BrandName { get; set; }
    }
}
