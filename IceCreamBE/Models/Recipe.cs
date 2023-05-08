namespace IceCreamBE.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public virtual Products Product { get; set; }
        public string Description { get; set; }
        public double Status { get; set; }
    }
}
