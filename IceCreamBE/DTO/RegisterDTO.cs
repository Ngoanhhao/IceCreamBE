namespace IceCreamBE.DTO
{
    public class RegisterDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime ExtensionDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
