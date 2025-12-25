using IIstudyWSClient;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace IIStudyWebApp.Controllers
{
    public class GuestController : Controller
    {
        [HttpGet]
        public IActionResult HomePage()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ViewBookCatalog()
        {

            ApiClient<List<Book>> client = new ApiClient<List<Book>>();

            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/GetBooks";
            List<Book> books = await client.GetAsync();
            return View(books);
        }

        public async Task<IActionResult> ViewBookPreview(string bookID)
        {
            ApiClient<ViewBookViewModel> client = new ApiClient<ViewBookViewModel>();
            client.Scheme = "http";
            client.Host= "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/GetBookFullView";
            ViewBookViewModel bookView = await client.GetAsync();
            return View(bookView);
        }
    }
}
