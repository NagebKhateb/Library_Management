namespace Domain
{
    public class SubCategory
    {
        public int SubCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // الفئة الفرعية تتبع لفئة أساسية واحدة
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

        public virtual List<Book> Books { get; set; } = new List<Book>();
    }
}
