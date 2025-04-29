namespace Domain
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public string? CoverImageUrl { get; set; }
        public bool IsActive { get; set; } = true;

        // الكتاب يتبع بشكل مباشر لفئة فرعية والتي بدورها تتبع لفئة أساسية
        // لا نربط الكتاب مع الفئة الأساسية لأن ذلك سيسبب لنا خلل في التجريد
        // فالعلاقة تسلسلية كالتالي: كتاب -> فئة فرعية -> فئة أساسية
        public int SubCategoryId { get; set; }
        public virtual SubCategory SubCategory { get; set; } = null!;

        // العلاقة بين الكتاب والكاتب متعدد إلى متعدد لذا نكسر العلاقة بالجدول التالي
        public virtual List<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    }
}
