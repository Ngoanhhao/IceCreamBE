namespace IceCreamBE.DTO
{
    public class StorageDTOCreate
    {
        public int ProductId { get; set; }
        public int quantity { get; set; }
        public DateTime last_order { get; set; }
    }
}
