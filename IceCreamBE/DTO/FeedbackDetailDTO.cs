namespace IceCreamBE.DTO
{
    public class FeedbackDetailDTO
    {
        public int Id { get; set; }
        public string FeedBackProduct { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
