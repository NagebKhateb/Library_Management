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

        [HttpGet("paged-books")]
        public async Task<IActionResult> GetBooksPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var (booksPaged, paginationMetaData) = await _repositoryQu.GetBooksPagedAsync(pageNumber, pageSize);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetaData));

            return Ok(booksPaged.Items);
        }

        [HttpGet("author-books/{authorId}")]
        public async Task<IActionResult> GetBooksPagedByAuthor(int authorId, int pageNumber = 1, int pageSize = 10)
        {
            var (books, metaData) = await _repositoryQu.GetBooksPagedByAuthorIdAsync(authorId, pageNumber, pageSize);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metaData));
            return Ok(books.Items);
        }

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

        [HttpPatch("archive/{bookId}")]
        public async Task<IActionResult> ArchiveBook(int bookId)
        {
            var archived = await _repositoryComm.ArchiveBookAsync(bookId);
            return archived ? Ok() : NotFound();
        }

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
