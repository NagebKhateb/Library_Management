namespace Domain
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // العلاقة بين الكتاب والكاتب متعدد إلى متعدد لذا نكسر العلاقة بالجدول التالي
        public virtual List<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    }
}
