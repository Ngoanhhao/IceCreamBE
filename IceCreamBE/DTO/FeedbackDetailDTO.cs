namespace IceCreamBE.DTO
{
    public class FeedbackDetailDTO
    {
        public int Id { get; set; }
        public string feedBack_product { get; set; }
        public string username { get; set; }
        public string full_name { get; set; }
        public string phone_number { get; set; }
        public string email { get; set; }
        public DateTime release_date { get; set; }
    }
}
