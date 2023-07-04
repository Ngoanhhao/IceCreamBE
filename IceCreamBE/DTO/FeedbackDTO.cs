namespace IceCreamBE.DTO
{
    public class FeedbackDTO
    {
        public int Id { get; set; }
        public string message { get; set; }
        public string full_name { get; set; }
        public string email { get; set; }
        public bool status { get; set; }
        public DateTime release_date { get; set; }
    }
}
