namespace IceCreamBE.DTO
{
    public class RecipeInDTO
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string? img { get; set; }
        public string description { get; set; }
        public bool status { get; set; }
    }
}
