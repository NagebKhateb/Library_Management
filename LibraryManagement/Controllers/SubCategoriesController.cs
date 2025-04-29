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

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] SubCategoryCreate dto)
        {
            await _repositoryComm.CreateSubCategoryAsync(dto);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repositoryComm.DeleteSubCategoryAsync(id);
            return result ? Ok() : NotFound();
        }

        [HttpGet("{subCategoryId}/main-category")]
        public async Task<IActionResult> GetMainCategory(int subCategoryId)
        {
            var category = await _repositoryQu.GetMainCategoryBySubCategoryIdAsync(subCategoryId);
            return category is null ? NotFound() : Ok(category);
        }

        [HttpGet("by-main-category")]
        public async Task<IActionResult> GetCategoriesWithSubCategories()
        {
            var result = await _repositoryQu.GetCategoriesWithSubCategoriesAsync();
            return Ok(result);
        }
    }
}
