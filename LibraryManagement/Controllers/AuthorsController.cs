using Domain.DTOs;
using Infrastructore.Repositories.Commands;
using Infrastructore.Repositories.Queries;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/author")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IRepositoryQu _repositoryQu;
        private readonly IRepositoryComm _repositoryComm;

        public AuthorsController(IRepositoryComm repositoryComm, IRepositoryQu repositoryQu)
        {
            this._repositoryQu = repositoryQu;
            this._repositoryComm = repositoryComm;
        }


        [HttpGet("{bookId}/authors")]
        public async Task<IActionResult> GetAuthorsByBook(int bookId)
        {
            var authors = await _repositoryQu.GetAuthorsByBookIdAsync(bookId);
            return Ok(authors);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorCreate dto)
        {
            await _repositoryComm.CreateAuthorAsync(dto);
            return Ok();
        }

        [HttpPut("update/{authorId}")]
        public async Task<IActionResult> UpdateAuthor(int authorId, [FromBody] AuthorUpdate dto)
        {
            var result = await _repositoryComm.UpdateAuthorAsync(authorId, dto);
            return Ok(result);
        }

        [HttpPatch("archive/{authorId}")]
        public async Task<IActionResult> ArchiveAuthor(int authorId)
        {
            var result = await _repositoryComm.ArchiveAuthorAsync(authorId);
            return Ok(result);
        }
    }
}
