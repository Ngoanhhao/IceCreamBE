namespace IceCreamBE.DTO
{
    public class AccountDetailOutDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Full_name { get; set; }
        public string Avatar { get; set; }
        public string Phone_number { get; set; }
        public DateTime Create_date { get; set; }
        public DateTime Extension_date { get; set; }
        public DateTime Expiration_date { get; set; }
    }
}
