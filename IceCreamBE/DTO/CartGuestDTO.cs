namespace IceCreamBE.DTO
{
    public class CartGuestDTO
    {
        public string full_name { get; set; }
        public string phone_number { get; set; }
        public string address { get; set; }
        public List<GuestCartItem> cart { get; set; }
    }

    public class GuestCartItem
    {
        public int product_id { get; set; }
        public int quantity { get; set; }
    }
}
