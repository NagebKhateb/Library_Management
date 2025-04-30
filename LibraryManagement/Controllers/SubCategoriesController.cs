using Domain.DTOs;
using Infrastructore.Repositories.Commands;
using Infrastructore.Repositories.Queries;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/sub-category")]
    [ApiController]
    public class SubCategoriesController : ControllerBase
    {
        private readonly IRepositoryQu _repositoryQu;
        private readonly IRepositoryComm _repositoryComm;


        public SubCategoriesController(IRepositoryComm repositoryComm, IRepositoryQu repositoryQu)
        {
            this._repositoryQu = repositoryQu;
            this._repositoryComm = repositoryComm;
        }


        /// <summary>
        /// Create a new subcategory
        /// </summary>
        /// <remarks>
        /// Creates a new subcategory with the provided details.
        /// The subcategory must be associated with an existing main category.
        /// </remarks>
        /// <param name="dto">Subcategory creation data</param>
        /// <returns>Success status</returns>
        /// <response code="200">Subcategory created successfully</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("create")]
        public async Task<IActionResult> CreateSubCategory([FromBody] SubCategoryCreate dto)
        {
            await _repositoryComm.CreateSubCategoryAsync(dto);
            return Ok();
        }

        /// <summary>
        /// Delete a subcategory
        /// </summary>
        /// <remarks>
        /// Permanently deletes a subcategory with the specified ID.
        /// Returns NotFound if the subcategory doesn't exist.
        /// </remarks>
        /// <param name="id">ID of the subcategory to delete</param>
        /// <returns>Delete result</returns>
        /// <response code="200">Subcategory deleted successfully</response>
        /// <response code="404">Subcategory not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            var result = await _repositoryComm.DeleteSubCategoryAsync(id);
            return result ? Ok() : NotFound();
        }

        /// <summary>
        /// Get main category for a subcategory
        /// </summary>
        /// <remarks>
        /// Retrieves the main category associated with the specified subcategory ID.
        /// </remarks>
        /// <param name="subCategoryId">ID of the subcategory</param>
        /// <returns>Main category details</returns>
        /// <response code="200">Returns the main category</response>
        /// <response code="404">Subcategory or main category not found</response>
        [HttpGet("{subCategoryId}/main-category")]
        public async Task<IActionResult> GetMainCategory(int subCategoryId)
        {
            var category = await _repositoryQu.GetMainCategoryBySubCategoryIdAsync(subCategoryId);
            return category is null ? NotFound() : Ok(category);
        }

        /// <summary>
        /// Get all categories with their subcategories
        /// </summary>
        /// <remarks>
        /// Retrieves a hierarchical list of all main categories
        /// with their associated subcategories.
        /// </remarks>
        /// <returns>List of categories with subcategories</returns>
        /// <response code="200">Returns the category hierarchy</response>
        [HttpGet("by-main-category")]
        public async Task<IActionResult> GetCategoriesWithSubCategories()
        {
            var result = await _repositoryQu.GetCategoriesWithSubCategoriesAsync();
            return Ok(result);
        }
    }
}
