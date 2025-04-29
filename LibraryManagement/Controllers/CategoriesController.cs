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
    }
}
