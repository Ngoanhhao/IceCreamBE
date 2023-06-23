namespace IceCreamBE.DTO
{
    public class UpdateAccountDTO
    {
        public int Id { get; set; }
        public int? roleID { get; set; }
        public string? full_name { get; set; }
        public string? avatar { get; set; }
        public string? phone_number { get; set; }
        public string? code { get; set; }
        public DateTime extension_date { get; set; }
        public DateTime expiration_date { get; set; }
    }
}
