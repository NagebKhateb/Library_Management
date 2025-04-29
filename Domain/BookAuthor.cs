﻿namespace Domain
{
    // جدول كسر العلاقة
    public class BookAuthor
    {
        public int Id { get; set; }

        public int BookId { get; set; }
        public virtual Book Book { get; set; } = null!;

        public int AuthorId { get; set; }
        public virtual Author Author { get; set; } = null!;
    }
}
