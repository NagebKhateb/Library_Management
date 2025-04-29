using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructore.DbConexts
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookAuthor>()
                .HasOne(b => b.Book)
                .WithMany(ba => ba.BookAuthors)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<BookAuthor>()
                .HasOne(a => a.Author)
                .WithMany(ba => ba.BookAuthors)
                .OnDelete(DeleteBehavior.ClientCascade);


            base.OnModelCreating(modelBuilder);
        }
    }
}
