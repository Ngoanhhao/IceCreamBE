namespace IceCreamBE.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        //public virtual Accounts Account { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
