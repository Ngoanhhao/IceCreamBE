namespace IceCreamBE.DTO
{
    public class RegisterDTO
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int roleID { get; set; }
        public string email { get; set; }
        public string full_name { get; set; }
        public string avatar { get; set; }
        public string phone_number { get; set; }
        public DateTime extension_date { get; set; }
        public DateTime expiration_date { get; set; }
    }
}
