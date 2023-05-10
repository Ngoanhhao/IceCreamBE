namespace IceCreamBE.DTO
{
    public class StorageDTO
    {
        public int Id { get; set; }
        public string product_name { get; set; }
        public string brand { get; set; }
        public int quantity { get; set; }
        public string img { get; set; }
        public DateTime last_order { get; set; }
    }
}
