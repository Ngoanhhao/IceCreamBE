namespace IceCreamBE.Models
{
    public class ResponseCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
