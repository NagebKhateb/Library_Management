namespace Domain.DTOs
{
    public class AuthorRead
    {
        public int AuthorId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
}
