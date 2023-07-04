namespace IceCreamBE.DTO
{
    public class ProductOutDTO
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string categories_name { get; set; }
        public double cost { get; set; }
        public double price { get; set; }
        public double? total { get; set; }
        public int quantity { get; set; }
        public string description { get; set; }
        public string img { get; set; }
        public int? discount_percent { get; set; } // percent
        public bool status { get; set; }
    }
}
