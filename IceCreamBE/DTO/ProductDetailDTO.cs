namespace IceCreamBE.DTO
{
    public class ProductDetailDTO
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string brand_name { get; set; }
        public double cost { get; set; }
        public double price { get; set; }
        public string description { get; set; }
        public string img { get; set; }
        public int? discount_percent { get; set; } // percent
        public double? total { get; set; }
        public bool status { get; set; }
    }
}
