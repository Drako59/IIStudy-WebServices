using Microsoft.AspNetCore.Mvc;

namespace IIStudyWebApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
