using Domain.DTOs;
using Infrastructore.Repositories.Commands;
using Infrastructore.Repositories.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LibraryManagement.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IRepositoryQu _repositoryQu;
        private readonly IRepositoryComm _repositoryComm;

        public BooksController(IRepositoryComm repositoryComm, IRepositoryQu repositoryQu)
        {
            this._repositoryQu = repositoryQu;
            this._repositoryComm = repositoryComm;
        }


        /// <summary>
        /// Creates a new book
        /// </summary>
        /// <remarks>
        /// Creates a new book with the provided details including title, description, 
        /// publication date, cover image URL, subcategory ID, and author IDs.
        /// </remarks>
        /// <param name="dto">Book creation data</param>
        /// <returns>Success message</returns>
        /// <response code="200">Book created successfully</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("create")]
        public async Task<IActionResult> CreateBook([FromBody] BookCreate dto)
        {
            try
            {
                await _repositoryComm.CreateBookAsync(dto);
                return Ok("Book created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get paginated list of all active books
        /// </summary>
        /// <remarks>
        /// Retrieves a paginated list of all active books with their subcategories, 
        /// categories, and authors. Pagination metadata is returned in X-Pagination header.
        /// </remarks>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Number of items per page (default: 10)</param>
        /// <returns>List of books</returns>
        /// <response code="200">Returns the paginated book list</response>
        [HttpGet("paged-books")]
        public async Task<IActionResult> GetBooksPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var (booksPaged, paginationMetaData) = await _repositoryQu.GetBooksPagedAsync(pageNumber, pageSize);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetaData));

            return Ok(booksPaged.Items);
        }

        /// <summary>
        /// Get paginated books by author ID
        /// </summary>
        /// <remarks>
        /// Retrieves a paginated list of active books written by a specific author. 
        /// Pagination metadata is returned in X-Pagination header.
        /// </remarks>
        /// <param name="authorId">ID of the author</param>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Number of items per page (default: 10)</param>
        /// <returns>List of books by author</returns>
        /// <response code="200">Returns the paginated book list</response>
        [HttpGet("author-books/{authorId}")]
        public async Task<IActionResult> GetBooksPagedByAuthor(int authorId, int pageNumber = 1, int pageSize = 10)
        {
            var (books, metaData) = await _repositoryQu.GetBooksPagedByAuthorIdAsync(authorId, pageNumber, pageSize);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metaData));
            return Ok(books.Items);
        }

        /// <summary>
        /// Get books filtered by category and subcategory
        /// </summary>
        /// <remarks>
        /// Retrieves a list of active books filtered by category ID and subcategory ID, 
        /// returning simplified book information.
        /// </remarks>
        /// <param name="categoryId">ID of the category</param>
        /// <param name="subCategoryId">ID of the subcategory</param>
        /// <returns>List of filtered books</returns>
        /// <response code="200">Returns the filtered book list</response>
        [HttpGet("featch")]
        public async Task<IActionResult> GetBooksByCategoryFilter([FromQuery] int categoryId, [FromQuery] int subCategoryId)
        {
            var books = await _repositoryQu.GetBooksByCategoryFilterAsync(categoryId, subCategoryId);
            return Ok(books);
        }

        /// <summary>
        /// Partially update a book
        /// </summary>
        /// <remarks>
        /// Updates specific fields of a book (description and cover image URL) 
        /// for the given book ID.
        /// </remarks>
        /// <param name="bookId">ID of the book to update</param>
        /// <param name="dto">Book update data</param>
        /// <returns>Update result</returns>
        /// <response code="200">Book updated successfully</response>
        /// <response code="500">Internal server error</response>
        [HttpPatch("update/{bookId}")]
        public async Task<IActionResult> UpdateBookPartial(int bookId, [FromBody] BookUpdate dto)
        {
            try
            {
                var result = await _repositoryComm.UpdateBookAsync(bookId, dto);
                return Ok(result);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Archive a book
        /// </summary>
        /// <remarks>
        /// Archives (soft deletes) a book by setting its IsActive flag to false 
        /// for the given book ID.
        /// </remarks>
        /// <param name="bookId">ID of the book to archive</param>
        /// <returns>Archive result</returns>
        /// <response code="200">Book archived successfully</response>
        /// <response code="404">Book not found</response>
        [HttpPatch("archive/{bookId}")]
        public async Task<IActionResult> ArchiveBook(int bookId)
        {
            var archived = await _repositoryComm.ArchiveBookAsync(bookId);
            return archived ? Ok() : NotFound();
        }

        /// <summary>
        /// Permanently delete a book
        /// </summary>
        /// <remarks>
        /// Permanently deletes a book and its author relationships from the database 
        /// for the given book ID.
        /// </remarks>
        /// <param name="bookId">ID of the book to delete</param>
        /// <returns>Delete result</returns>
        /// <response code="200">Book deleted successfully</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("delete/{bookId}")]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            try
            {
                var result = await _repositoryComm.DeleteBookAsync(bookId);
                return Ok("Book deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
