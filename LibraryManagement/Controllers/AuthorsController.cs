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
    }
}
