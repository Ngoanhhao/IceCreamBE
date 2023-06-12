namespace IceCreamBE.DTO
{
    public class StorageOutDTO
    {
        public int Id { get; set; }
        public string product_name { get; set; }
        public int quantity { get; set; }
        public DateTime last_order { get; set; }
    }
}
