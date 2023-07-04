using MailKit.Net.Imap;

namespace IceCreamBE.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? img { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
    }
}
