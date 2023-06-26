namespace IceCreamBE.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string FeedBackProduct { get; set; }
        public string? FullName { get; set; }
        //public virtual Accounts Account { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
