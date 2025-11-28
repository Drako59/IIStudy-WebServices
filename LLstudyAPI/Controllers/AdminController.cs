using LLstudyWS.ORM.Repositorys;
using Microsoft.AspNetCore.Mvc;

namespace LLstudyWS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        RepositoryUOW repositoryUOW;
        public AdminController() {
            this.repositoryUOW = new RepositoryUOW();
        }




    }
}
