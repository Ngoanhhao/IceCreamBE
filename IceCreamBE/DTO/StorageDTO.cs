namespace IceCreamBE.DTO
{
    public class StorageDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Brand { get; set; }
        public int Quantity { get; set; }
        public string Img { get; set; }
        public DateTime LastOrder { get; set; }
    }
}
