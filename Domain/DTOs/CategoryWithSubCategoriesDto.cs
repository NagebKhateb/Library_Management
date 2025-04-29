namespace Domain.DTOs
{
    public class CategoryWithSubCategoriesDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public List<SubCategorySimpleDto> SubCategories { get; set; } = new();
    }
}
