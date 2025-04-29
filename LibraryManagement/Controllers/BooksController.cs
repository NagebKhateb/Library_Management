using Infrastructore.Repositories.Commands;
using Infrastructore.Repositories.Queries;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IRepositoryQu _repositoryQu;
        private readonly IRepositoryComm _repositoryComm;

        public BooksController(IRepositoryComm repositoryComm, IRepositoryQu repositoryQu)
        {
            this._repositoryQu = repositoryQu;
            this._repositoryComm = repositoryComm;
        }
    }
}
