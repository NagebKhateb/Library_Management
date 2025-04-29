using Domain.DTOs;

namespace Infrastructore.Repositories.Commands
{
    public interface IRepositoryComm
    {
        Task CreateBookAsync(BookCreate dto);

        Task<bool> UpdateBookAsync(int bookId, BookUpdate dto);

        Task<bool> ArchiveBookAsync(int bookId);

        Task<bool> DeleteBookAsync(int bookId);



        Task CreateAuthorAsync(AuthorCreate dto);

        Task<bool> UpdateAuthorAsync(int authorId, AuthorUpdate dto);

        Task<bool> ArchiveAuthorAsync(int authorId);



        Task CreateCategoryAsync(string name);

        Task ArchiveCategoryWithSubCategoriesAsync(int categoryId);

        Task DeleteCategoryWithSubCategoriesAsync(int categoryId);



        Task CreateSubCategoryAsync(SubCategoryCreate dto);

        Task<bool> DeleteSubCategoryAsync(int subCategoryId);




    }
}
