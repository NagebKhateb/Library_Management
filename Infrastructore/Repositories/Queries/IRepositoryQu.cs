using Domain.Pagination;
using Domain;
using Domain.DTOs;

namespace Infrastructore.Repositories.Queries
{
    public interface IRepositoryQu
    {
        Task<(PaginatedList<Book>, PaginationMetaData)> GetBooksPagedAsync(int pageNumber, int pageSize);

        Task<(PaginatedList<Book>, PaginationMetaData)> GetBooksPagedByAuthorIdAsync(int authorId, int pageNumber, int pageSize);

        Task<List<BookSimpleDto>> GetBooksByCategoryFilterAsync(int categoryId, int subCategoryId);



        Task<List<AuthorRead>> GetAuthorsByBookIdAsync(int bookId);



        Task<Category?> GetMainCategoryBySubCategoryIdAsync(int subCategoryId);

        Task<List<CategoryWithSubCategoriesDto>> GetCategoriesWithSubCategoriesAsync();



        Task<List<QuestionDto>> GetRecentQuestionsAsync();

        Task<QuestionDto?> GetQuestionByIdAsync(int questionId);
    }
}
