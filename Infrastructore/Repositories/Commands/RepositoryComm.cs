using Domain;
using Domain.DTOs;
using Infrastructore.DbConexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Infrastructore.Repositories.Commands
{
    public class RepositoryComm : IRepositoryComm
    {
        private readonly LibraryDbContext _dbContext;
        public RepositoryComm(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task CreateBookAsync(BookCreate dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                Description = dto.Description,
                PublishedDate = dto.PublishedDate,
                CoverImageUrl = dto.CoverImageUrl,
                SubCategoryId = dto.SubCategoryId,
                BookAuthors = dto.AuthorIds.Select(id => new BookAuthor { AuthorId = id }).ToList()
            };

            _dbContext.Books.Add(book);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateBookAsync(int bookId, BookUpdate dto)
        {
            var book = await _dbContext.Books.FindAsync(bookId);

            if (book == null || !book.IsActive)
                throw new Exception("Book not found or is archived.");

            book.Description = dto.Description;
            book.CoverImageUrl = dto.CoverImageUrl;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ArchiveBookAsync(int bookId)
        {
            var book = await _dbContext.Books.FindAsync(bookId);
            if (book == null || !book.IsActive) return false;

            book.IsActive = false;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBookAsync(int bookId)
        {
            var book = await _dbContext.Books
                .Include(b => b.BookAuthors)
                .FirstOrDefaultAsync(b => b.BookId == bookId);

            if (book == null)
                throw new Exception("Book not found.");

            // نحذف العلاقات مع المؤلفين
            _dbContext.BookAuthors.RemoveRange(book.BookAuthors);

            // ثم نحذف الكتاب نفسه
            _dbContext.Books.Remove(book);

            await _dbContext.SaveChangesAsync();
            return true;
        }



        public async Task CreateAuthorAsync(AuthorCreate dto)
        {
            var author = new Author
            {
                Name = dto.Name,
                Bio = dto.Bio,
                Country = dto.Country
            };

            _dbContext.Authors.Add(author);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAuthorAsync(int authorId, AuthorUpdate dto)
        {
            var author = await _dbContext.Authors.FindAsync(authorId);

            if (author == null || !author.IsActive)
                throw new Exception("Author not found or archived.");

            author.Bio = dto.Bio;
            author.Country = dto.Country;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ArchiveAuthorAsync(int authorId)
        {
            var author = await _dbContext.Authors.FindAsync(authorId);

            if (author == null || !author.IsActive)
                return false;

            author.IsActive = false;
            await _dbContext.SaveChangesAsync();
            return true;
        }



        public async Task CreateCategoryAsync(string name)
        {
            var sql = "EXEC sp_CreateCategory @Name";
            await _dbContext.Database.ExecuteSqlRawAsync(sql, new SqlParameter("@Name", name));
        }

        public async Task ArchiveCategoryWithSubCategoriesAsync(int categoryId)
        {
            var sql = "EXEC sp_ArchiveCategoryWithSubCategories @CategoryId";
            await _dbContext.Database.ExecuteSqlRawAsync(sql, new SqlParameter("@CategoryId", categoryId));
        }

        public async Task DeleteCategoryWithSubCategoriesAsync(int categoryId)
        {
            var sql = "EXEC sp_DeleteCategoryWithSubCategories @CategoryId";
            await _dbContext.Database.ExecuteSqlRawAsync(sql, new SqlParameter("@CategoryId", categoryId));
        }



        public async Task CreateSubCategoryAsync(SubCategoryCreate dto)
        {
            var subCategory = new SubCategory
            {
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                IsActive = true
            };

            _dbContext.SubCategories.Add(subCategory);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteSubCategoryAsync(int subCategoryId)
        {
            var subCategory = await _dbContext.SubCategories.FindAsync(subCategoryId);

            if (subCategory == null)
                return false;

            _dbContext.SubCategories.Remove(subCategory);
            await _dbContext.SaveChangesAsync();
            return true;
        }


    }
}
