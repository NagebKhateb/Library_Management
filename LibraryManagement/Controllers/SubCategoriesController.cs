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
    }
}
