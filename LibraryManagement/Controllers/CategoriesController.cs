using Infrastructore.Repositories.Commands;
using Infrastructore.Repositories.Queries;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IRepositoryQu _repositoryQu;
        private readonly IRepositoryComm _repositoryComm;

        public CategoriesController(IRepositoryComm repositoryComm, IRepositoryQu repositoryQu)
        {
            this._repositoryQu = repositoryQu;
            this._repositoryComm = repositoryComm;
        }


        /// <summary>
        /// Create a new category
        /// </summary>
        /// <remarks>
        /// Creates a new book category with the specified name.
        /// Uses a stored procedure to perform the creation.
        /// </remarks>
        /// <param name="name">Name of the category to create</param>
        /// <returns>Success message</returns>
        /// <response code="200">Category created successfully</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromBody] string name)
        {
            await _repositoryComm.CreateCategoryAsync(name);
            return Ok("Category created successfully.");
        }

        /// <summary>
        /// Archive a category and its subcategories
        /// </summary>
        /// <remarks>
        /// Archives (soft deletes) a category and all its associated subcategories.
        /// Uses a stored procedure to handle the archival process.
        /// Archived items remain in the system but are marked as inactive.
        /// </remarks>
        /// <param name="id">ID of the category to archive</param>
        /// <returns>Success message</returns>
        /// <response code="200">Category archived successfully</response>
        /// <response code="404">Category not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPatch("archive/{id}")]
        public async Task<IActionResult> ArchiveCategory(int id)
        {
            await _repositoryComm.ArchiveCategoryWithSubCategoriesAsync(id);
            return Ok("Category archived successfully.");
        }

        /// <summary>
        /// Permanently delete a category and its subcategories
        /// </summary>
        /// <remarks>
        /// Completely removes a category and all its associated subcategories from the system.
        /// Uses a stored procedure to handle the deletion process.
        /// This action cannot be undone.
        /// </remarks>
        /// <param name="id">ID of the category to delete</param>
        /// <returns>Success message</returns>
        /// <response code="200">Category deleted successfully</response>
        /// <response code="404">Category not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _repositoryComm.DeleteCategoryWithSubCategoriesAsync(id);
            return Ok("Category deleted successfully.");
        }
    }
}
