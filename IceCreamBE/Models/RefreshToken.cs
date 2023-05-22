namespace IceCreamBE.Models
{
    public class RefreshToken
    {
        public int id { get; set; }
        public int userId { get; set; }
        public virtual Accounts user { get; set; }
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public bool isUsed { get; set; }
        public DateTime createDate { get; set; }
        public DateTime expirationDate { get; set; }
    }
}
