namespace Domain.DTOs
{
    public class BookCreate
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public string? CoverImageUrl { get; set; }

        public int SubCategoryId { get; set; }

        public List<int> AuthorIds { get; set; } = new();
    }
}
