namespace Domain.DTOs
{
    public class BookUpdate
    {
        public string Description { get; set; } = string.Empty;
        public string? CoverImageUrl { get; set; }
    }
}
