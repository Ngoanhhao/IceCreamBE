namespace IceCreamBE.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string FeedBackProduct { get; set; }
        public int AccountID { get; set; }
        public Accounts Account { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
