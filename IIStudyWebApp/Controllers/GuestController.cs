using IIstudyWSClient;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using LLStudy_Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Net;
using System.Security.Cryptography.X509Certificates;

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

        public async Task<IActionResult> viewSignUpPage()
        {
            return View();
        }

        public async Task<IActionResult> viewSignInPage()
        {
            return View();
        }
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
            client.AddParameter("bookID", bookID);

            ViewBookViewModel bookView = await client.GetAsync();
            return View(bookView);
        }

        public async Task<IActionResult> ViewExams(string year = null, string subjectID = null, int pages = 0)
        {
            ApiClient<ViewExamsModel> client = new ApiClient<ViewExamsModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/GetExams";
            client.AddParameter("year", year);
            client.AddParameter("subjectID",subjectID);
            client.AddParameter("pages", pages);

            ViewExamsModel examView = await client.GetAsync();
            return View(examView);
        }

        public async Task<IActionResult> ViewCalender()
        {
            ApiClient<List<Event>> client = new ApiClient<List<Event>>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/GetExams";

            List<Event> calender = await client.GetAsync();
            return View(calender);
        }

        public async Task<IActionResult> SignIn(SignInViewModel SignInModel)
        {
            ApiClient<SignInViewModel> client = new ApiClient<SignInViewModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/SignIn";


            ApiResultModel<Registered> success = client.PostAsyncRet<SignInViewModel, Registered>(SignInModel).Result;


            if (success.Success && success.Data != null)
            {
                Console.WriteLine($@"{success.Data.RegisteredID}");

                HttpContext.Session.SetString("RegisteredID", success.Data.RegisteredID);
                return RedirectToAction("RegisteredHomePage", "Registered");
            }

            return RedirectToAction("ViewSignInPage");
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(Registered reg)
        {
            ApiClient<Registered> client = new ApiClient<Registered>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/SignUp";

            reg.Role = "User";
            reg.RegisteredID = "6";
            reg.RegisteredSalt = " ";
            reg.ImagePath = "None";
             ApiResultModel<Registered> success = client.PostAsyncRet<Registered, Registered>(reg).Result;

            //888888888888888888888888888888888888

            //888888888888888888888888888888888888
            //ApiResultModel<string> success = client.PostAsync(reg).Result;
            await Console.Out.WriteLineAsync(  "here****************************************");
            await Console.Out.WriteLineAsync(success.Success + "HEREHREREHREHEHREHRHEEH");

            Console.WriteLine($@"{success.Data.RegisteredID}");
            if (success.Success && success.Data != null)
            {

                HttpContext.Session.SetString("RegisteredID", success.Data.RegisteredID);
                return RedirectToAction("RegisteredHomePage", "Registered");
            }

            return RedirectToAction("ViewSignUpPage");
            //if (!success)
            //    return View("Failed to sign up.");
            //return View("User has been signed up.");

        }

        private async Task<string> SendReader()
        {
            return null;
        }
    }
}
