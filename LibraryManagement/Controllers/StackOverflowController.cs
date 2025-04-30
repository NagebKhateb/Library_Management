using Infrastructore.Repositories.Queries;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/stackoverflow")]
    [ApiController]
    public class StackOverflowController : ControllerBase
    {
        private readonly IRepositoryQu _repositoryQu;
        private readonly ILogger<StackOverflowController> _logger;


        public StackOverflowController(IRepositoryQu repositoryQu, ILogger<StackOverflowController> logger)
        {
            _repositoryQu = repositoryQu;
            _logger = logger;
        }


        /// <summary>
        /// Get recent StackOverflow questions
        /// </summary>
        /// <remarks>
        /// Retrieves a list of the most recent questions from StackOverflow.
        /// Logs the operation and any errors that occur during the process.
        /// </remarks>
        /// <returns>List of recent questions</returns>
        /// <response code="200">Returns the list of recent questions</response>
        /// <response code="500">Internal server error occurred</response>
        [HttpGet("questions")]
        public async Task<IActionResult> GetRecentQuestions()
        {
            _logger.LogInformation("Fetching recent questions from StackOverflow");

            try
            {
                var questions = await _repositoryQu.GetRecentQuestionsAsync();
                _logger.LogInformation("Retrieved {QuestionCount} questions", questions.Count);

                return Ok(questions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching StackOverflow questions");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get details of a specific question
        /// </summary>
        /// <remarks>
        /// Retrieves complete details for a specific StackOverflow question by its ID.
        /// Logs the operation, including when questions are not found, and any errors.
        /// </remarks>
        /// <param name="id">The ID of the question to retrieve</param>
        /// <returns>Question details</returns>
        /// <response code="200">Returns the requested question details</response>
        /// <response code="404">Question with the specified ID was not found</response>
        /// <response code="500">Internal server error occurred</response>
        [HttpGet("questions/{id}")]
        public async Task<IActionResult> GetQuestionDetails(int id)
        {
            _logger.LogInformation("Fetching details for question ID: {QuestionId}", id);

            try
            {
                var question = await _repositoryQu.GetQuestionByIdAsync(id);

                if (question == null)
                {
                    _logger.LogWarning("Question with ID {QuestionId} not found", id);
                    return NotFound();
                }

                _logger.LogInformation("Successfully retrieved question: {QuestionTitle}",
                    question.Title);

                return Ok(question);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching question details for ID {QuestionId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
