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


        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromBody] string name)
        {
            await _repositoryComm.CreateCategoryAsync(name);
            return Ok("Category created successfully.");
        }

        [HttpPatch("archive/{id}")]
        public async Task<IActionResult> ArchiveCategory(int id)
        {
            await _repositoryComm.ArchiveCategoryWithSubCategoriesAsync(id);
            return Ok("Category archived successfully.");
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _repositoryComm.DeleteCategoryWithSubCategoriesAsync(id);
            return Ok("Category deleted successfully.");
        }
    }
}
