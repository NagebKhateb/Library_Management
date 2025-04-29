using Infrastructore.Repositories.Commands;
using Infrastructore.Repositories.Queries;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StackOverflowController : ControllerBase
    {
        private readonly IRepositoryQu _repositoryQu;
        private readonly IRepositoryComm _repositoryComm;

        public StackOverflowController(IRepositoryComm repositoryComm, IRepositoryQu repositoryQu)
        {
            this._repositoryQu = repositoryQu;
            this._repositoryComm = repositoryComm;
        }
    }
}
