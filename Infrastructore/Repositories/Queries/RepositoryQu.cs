using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using Domain.DTOs;
using Domain.Pagination;
using Infrastructore.DbConexts;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Infrastructore.Repositories.Queries
{
    public class RepositoryQu : IRepositoryQu
    {
        private readonly LibraryDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        public RepositoryQu(LibraryDbContext dbContext, IMapper mapper, HttpClient httpClient)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.stackexchange.com/2.3/");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "LibraryApp/1.0");
        }


        public async Task<(PaginatedList<Book>, PaginationMetaData)> GetBooksPagedAsync(int pageNumber, int pageSize)
        {
            var query = _dbContext.Books
                .Include(b => b.SubCategory).ThenInclude(sc => sc.Category)
                .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
                .Where(b => b.IsActive)
                .AsQueryable();

            var paginatedList = await PaginatedList<Book>.CreateAsync(query, pageNumber, pageSize);

            var metaData = new PaginationMetaData(pageSize, pageNumber, paginatedList.TotalCount);

            return (paginatedList, metaData);
        }

        public async Task<(PaginatedList<Book>, PaginationMetaData)> GetBooksPagedByAuthorIdAsync(int authorId, int pageNumber, int pageSize)
        {
            var query = _dbContext.Books
                .Where(b => b.IsActive && b.BookAuthors.Any(ba => ba.AuthorId == authorId))
                .Include(b => b.SubCategory).ThenInclude(sc => sc.Category)
                .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
                .AsQueryable();

            var paginatedList = await PaginatedList<Book>.CreateAsync(query, pageNumber, pageSize);

            var metaData = new PaginationMetaData(pageSize, pageNumber, paginatedList.TotalCount);

            return (paginatedList, metaData);
        }



        public async Task<List<AuthorRead>> GetAuthorsByBookIdAsync(int bookId)
        {
            var authors = await _dbContext.BookAuthors
                .Where(ba => ba.BookId == bookId && ba.Author.IsActive)
                .Select(ba => ba.Author)
                .ProjectTo<AuthorRead>(_mapper.ConfigurationProvider)
                .ToListAsync();
            //  لتقليل الاستعلامات وتحويل النتيجة مباشرة في قاعدة البيانات بدلاً من تحويلها في الذاكرة ProjectTo استخدمنا
            
            return authors;
        }



        public async Task<Category?> GetMainCategoryBySubCategoryIdAsync(int subCategoryId)
        {
            var subCategory = await _dbContext.SubCategories
                .Where(sc => sc.SubCategoryId == subCategoryId)
                .Select(sc => sc.Category)
                .FirstOrDefaultAsync();

            return subCategory;
        }

        public async Task<List<CategoryWithSubCategoriesDto>> GetCategoriesWithSubCategoriesAsync()
        {
            var responsiveCategory = await _dbContext.Categories
                .Where(c => c.IsActive)
                .Include(c => c.SubCategories.Where(sc => sc.IsActive))
                .Select(c => new CategoryWithSubCategoriesDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.Name,
                    SubCategories = c.SubCategories
                        .Select(sc => new SubCategorySimpleDto
                        {
                            SubCategoryId = sc.SubCategoryId,
                            SubCategoryName = sc.Name
                        })
                        .ToList()
                })
                .ToListAsync();

            return responsiveCategory;
        }



        public async Task<List<QuestionDto>> GetRecentQuestionsAsync()
        {
            var url = "https://api.stackexchange.com/2.3/questions?order=desc&sort=activity&site=stackoverflow&pagesize=50";
            
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = await JsonSerializer.DeserializeAsync<StackOverflowResponseDto>(stream, options);

            return result?.Items ?? new List<QuestionDto>();
        }

        public async Task<QuestionDto?> GetQuestionByIdAsync(int questionId)
        {
            var url = $"https://api.stackexchange.com/2.3/questions/{questionId}?order=desc&sort=activity&site=stackoverflow&filter=withbody";
            
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = await JsonSerializer.DeserializeAsync<StackOverflowResponseDto>(stream, options);

            return result?.Items?.FirstOrDefault();
        }
    }
}
