namespace Domain
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public virtual List<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
    }
}
