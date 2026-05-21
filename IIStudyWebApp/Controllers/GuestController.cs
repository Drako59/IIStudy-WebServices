using IIstudyWSClient;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using LLStudy_Models.ViewModels.Guest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using NuGet.Protocol.Plugins;
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

            if(books == null)
            {
                books = new List<Book>();
            }
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

            if(bookView == null)
            {
                return StatusCode(404);
            }

            return View(bookView);
        }

        //public async Task<IActionResult> ViewExams(string year = null, string subjectID = null, int pages = 0) //Not in use
        //{
        //    ApiClient<ViewExamsModel> client = new ApiClient<ViewExamsModel>();
        //    client.Scheme = "http";
        //    client.Host = "localhost";
        //    client.Port = 5049;
        //    client.Path = "api/Guest/GetExams";
        //    client.AddParameter("year", year);
        //    client.AddParameter("subjectID",subjectID);
        //    client.AddParameter("pages", pages);

        //    ViewExamsModel examView = await client.GetAsync();
        //    return View(examView);
        //}
        [HttpGet]

        public async Task<IActionResult> ViewCalendar()
        {
            
            //ApiClient<List<Event>> client = new ApiClient<List<Event>>();
            //client.Scheme = "http";
            //client.Host = "localhost";
            //client.Port = 5049;
            //client.Path = "api/Guest/GetExams";

            //List<Event> calender = await client.GetAsync();
            return View();
        }

        public async Task<IActionResult> SignIn(SignInViewModel SignInModel)
        {
            ApiClient<SignInViewModel> client = new ApiClient<SignInViewModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/SignIn";


            ApiResultModel<Registered> success = client.PostAsyncRet<SignInViewModel, Registered>(SignInModel).Result;

            
            if (success.Success && success.Data != null && Convert.ToInt64(success.Data.RegisteredID) > 0)
            {
                //Console.WriteLine($@"{success.Data.RegisteredID}");

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

            //reg.Validate();
            //var dict = reg.AllErrors();
            //Console.WriteLine(reg.Password);
            //foreach (KeyValuePair<string, List<string>> pair in dict)
            //{
            //    Console.WriteLine(pair.Key);
            //    foreach (string error in pair.Value)
            //    {
            //        Console.WriteLine($@"\t{error}");
            //    }
            //}
            
             ApiResultModel<Registered> success = client.PostAsyncRet<Registered, Registered>(reg).Result;

            //888888888888888888888888888888888888

            //888888888888888888888888888888888888
            //ApiResultModel<string> success = client.PostAsync(reg).Result;
            await Console.Out.WriteLineAsync(  "here****************************************");
            await Console.Out.WriteLineAsync(success.Success + "HEREHREREHREHEHREHRHEEH");

            //Console.WriteLine($@"{success.Data.RegisteredID}");
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

        [HttpGet]
        public async Task<IActionResult> GetBookImage(string bookID)
        {
            ApiClient<Registered> client = new ApiClient<Registered>();

            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/GetBookImage";
            client.AddParameter("bookID", bookID);
            ApiFileResultModel file = client.GetAsyncFile().Result;

            if(file == null)
            {
                return StatusCode(404);
            }

            return File(file.Bytes, file.ContentType);
        }

        [HttpGet]
        public async Task<IActionResult> ViewExamsPage()
        {
            ApiClient<Dictionary<string,string>> client = new ApiClient<Dictionary<string,string>>();

            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/GetAllSubjectsDict";
            Dictionary<string, string> subjects = await client.GetAsync();

            if(subjects == null)
            {
                subjects = new Dictionary<string, string>();
            }

            return View(subjects);


        }

        [HttpGet]
        public async Task<IActionResult> ViewExamYearsPage(string subjectID)
        {
            ApiClient<ExamsSubjectYearViewModel> client = new ApiClient<ExamsSubjectYearViewModel>();

            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/ExamsYearsListBySubject";
            client.AddParameter("subjectID", subjectID);
            ExamsSubjectYearViewModel years = await client.GetAsync();
            if(years == null)
            {
                years = new ExamsSubjectYearViewModel();
            }

            return View(years);


        }


        [HttpGet]
        public async Task<IActionResult> ViewExamsBySubjectAndYear(string subjectID, string year)
        {
            ApiClient<List<ExamDetailsWeb>> client = new ApiClient<List<ExamDetailsWeb>>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/ViewExamsBySubjectAndYear";
            client.AddParameter("subjectID", subjectID);
            client.AddParameter("year", year);
            List<ExamDetailsWeb> exams = await client.GetAsync();
            if(exams == null)
            {
                exams = new List<ExamDetailsWeb>();
            }


            return View(exams);
        }

        [HttpGet]
        public async Task<IActionResult> ViewExamFile(string examID)
        {
            ApiClient<bool> client = new ApiClient<bool>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/GetExamFile";
            client.AddParameter("examID", examID);
            ApiFileResultModel examFile = await client.GetAsyncFile();

            if(examFile == null)
            {
                return StatusCode(404);
            }
            return File(examFile.Bytes, examFile.ContentType);

        }

        [HttpGet]

        public async Task<IActionResult> ViewSolutionsByExam(string examID)
        {
            ApiClient<List<SolutionDetailsWeb>> client = new ApiClient<List<SolutionDetailsWeb>>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/GetSolutionsByExam";
            client.AddParameter("examID", examID);

            List<SolutionDetailsWeb> solutions = await client.GetAsync();

            if(solutions == null)
            {
                solutions = new List<SolutionDetailsWeb>();
            }
            return View(solutions);

        }

        [HttpGet]
        public async Task<IActionResult> ViewSolutionFile(string solutionID)
        {
            ApiClient<bool> client = new ApiClient<bool>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/GetSolutionFile";
            client.AddParameter("solutionID", solutionID);
            ApiFileResultModel solutionFile = await client.GetAsyncFile();

            if (solutionFile == null)
            {
                return StatusCode(404);
            }
            return File(solutionFile.Bytes, solutionFile.ContentType);

        }

        private async Task<string> SendReader()
        {
            return null;
        }
    }
}
