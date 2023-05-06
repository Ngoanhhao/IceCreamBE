namespace IceCreamBE.Models
{
    public class Storage
    {
        public int ProductID { get; set; }
        public Products Product { get; set; }
        public int Quantity { get; set; }
        public DateTime LastOrder { get; set; }
    }
}
