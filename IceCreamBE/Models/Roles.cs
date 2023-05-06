namespace IceCreamBE.Models
{
    public class Roles
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public ICollection<Accounts> Accounts { get; set; }
    }
}
