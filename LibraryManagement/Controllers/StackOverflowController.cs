using Infrastructore.Repositories.Queries;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StackOverflowController : ControllerBase
    {
        private readonly IRepositoryQu _repositoryQu;


        public StackOverflowController(IRepositoryQu repositoryQu)
        {
            this._repositoryQu = repositoryQu;
        }

        [HttpGet("questions")]
        public async Task<IActionResult> GetRecentQuestions()
        {
            var questions = await _repositoryQu.GetRecentQuestionsAsync();
            return Ok(questions);
        }

        [HttpGet("questions/{id}")]
        public async Task<IActionResult> GetQuestionDetails(int id)
        {
            var question = await _repositoryQu.GetQuestionByIdAsync(id);
            if (question == null)
                return NotFound();

            return Ok(question);
        }
    }
}
