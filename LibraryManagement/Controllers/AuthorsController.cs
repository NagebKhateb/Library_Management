using Domain.DTOs;
using Infrastructore.Repositories.Commands;
using Infrastructore.Repositories.Queries;
using LibraryManagement.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/author")]
    [ApiController]
    [ServiceFilter(typeof(LibraryActionFilter))]
    public class AuthorsController : ControllerBase
    {
        private readonly IRepositoryQu _repositoryQu;
        private readonly IRepositoryComm _repositoryComm;

        public AuthorsController(IRepositoryComm repositoryComm, IRepositoryQu repositoryQu)
        {
            this._repositoryQu = repositoryQu;
            this._repositoryComm = repositoryComm;
        }


        /// <summary>
        /// Get authors by book ID
        /// </summary>
        /// <remarks>
        /// Retrieves a list of all authors associated with a specific book.
        /// </remarks>
        /// <param name="bookId">ID of the book</param>
        /// <returns>List of authors</returns>
        /// <response code="200">Returns the list of authors</response>
        [HttpGet("{bookId}/authors")]
        public async Task<IActionResult> GetAuthorsByBook(int bookId)
        {
            var authors = await _repositoryQu.GetAuthorsByBookIdAsync(bookId);
            return Ok(authors);
        }

        /// <summary>
        /// Create a new author
        /// </summary>
        /// <remarks>
        /// Adds a new author to the system with the provided details.
        /// </remarks>
        /// <param name="dto">Author creation data</param>
        /// <returns>Success status</returns>
        /// <response code="200">Author created successfully</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("create")]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorCreate dto)
        {
            await _repositoryComm.CreateAuthorAsync(dto);
            return Ok();
        }

        /// <summary>
        /// Update author information
        /// </summary>
        /// <remarks>
        /// Updates all fields of an existing author with the provided data.
        /// </remarks>
        /// <param name="authorId">ID of the author to update</param>
        /// <param name="dto">Author update data</param>
        /// <returns>Update result</returns>
        /// <response code="200">Author updated successfully</response>
        /// <response code="404">Author not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("update/{authorId}")]
        public async Task<IActionResult> UpdateAuthor(int authorId, [FromBody] AuthorUpdate dto)
        {
            var result = await _repositoryComm.UpdateAuthorAsync(authorId, dto);
            return Ok(result);
        }

        /// <summary>
        /// Archive an author
        /// </summary>
        /// <remarks>
        /// Marks an author as archived (soft delete) by setting their IsActive flag to false.
        /// Archived authors remain in the system but are not visible in normal operations.
        /// </remarks>
        /// <param name="authorId">ID of the author to archive</param>
        /// <returns>Archive result</returns>
        /// <response code="200">Author archived successfully</response>
        /// <response code="404">Author not found</response>
        [HttpPatch("archive/{authorId}")]
        public async Task<IActionResult> ArchiveAuthor(int authorId)
        {
            var result = await _repositoryComm.ArchiveAuthorAsync(authorId);
            return Ok(result);
        }
    }
}
