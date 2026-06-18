using IIstudyWSClient;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using LLStudy_Models.ViewModels.Guest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Protocol.Plugins;
using System.Data;
using System.Net;
using System.Runtime.CompilerServices;
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
            ViewBag.WasSubmitted = false;
            return View();
        }

        public async Task<IActionResult> viewSignInPage()
        {
            ViewBag.WrongDetails = false;
            ViewBag.ErroOccured = false;
            return View();
        }
        public async Task<IActionResult> ViewBookCatalog(string? search = null, int pageNumber = 1,string? subjectID = null, int? minPrice = null, int? maxPrice = null, bool inStock = false, bool isOnline = false, bool isPhysical = false)
        {

            ApiClient<ViewBookCatalogModel> client = new ApiClient<ViewBookCatalogModel>();

            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/GetBooksByFilter";
            client.AddParameter("pageNumber", pageNumber);
            if(subjectID != null)
                client.AddParameter("subjectID", subjectID);
            client.AddParameter("minPrice", minPrice);
            client.AddParameter("maxPrice", maxPrice);
            client.AddParameter("inStock", inStock);
            client.AddParameter("isOnline", isOnline);
            client.AddParameter("isPhysical", isPhysical);
            client.AddParameter("search", search);





            ViewBookCatalogModel books = await client.GetAsync();

            if(books == null)
            {
                books = new ViewBookCatalogModel();
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


            ApiResultModel<Registered> success = await client.PostAsyncRet<SignInViewModel, Registered>(SignInModel);


            if (success.Success && success.Data != null && Convert.ToInt64(success.Data.RegisteredID) > 0)
            {
                //Console.WriteLine($@"{success.Data.RegisteredID}");

                HttpContext.Session.SetString("RegisteredID", success.Data.RegisteredID);
                return RedirectToAction("RegisteredHomePage", "Registered");
            }
            else {
                ViewBag.WrongDetails = true;
                return View("ViewSignInPage"); 
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(Registered reg)
        {
            ApiClient<SignUpResultModel> client = new ApiClient<SignUpResultModel>();
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
            
             ApiResultModel<SignUpResultModel> success = await client.PostAsyncRet<Registered, SignUpResultModel>(reg);

            if (success.Success && success.Data.RegisteredID != "0" && !success.Data.EmailAlreadyInUse && !success.Data.UserNameAlreadyInUse)
            {
                HttpContext.Session.SetString("RegisteredID", success.Data.RegisteredID);
                return RedirectToAction("RegisteredHomePage", "Registered");
            }
            else if (success.Success && success.Data.EmailAlreadyInUse && success.Data.UserNameAlreadyInUse)
            {
                ViewBag.WasSubmitted = true;
                return View("ViewSignUpPage", success.Data);
            }
            else
            {
                SignUpResultModel result = new SignUpResultModel()
                {
                    Birth = reg.Birth,
                    Email = reg.Email,
                    IsBanned = reg.IsBanned,
                    Password = reg.Password,
                    ImagePath = reg.ImagePath,
                    Phone = reg.Phone,
                    Role = reg.Role,
                    UserName = reg.UserName,
                    RegisteredSalt = reg.RegisteredSalt,
                    RegisteredID = reg.RegisteredID
                };
                ViewBag.WasSubmitted = true;
                return View("ViewSignUpPage", result);
            }

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

        [HttpGet]
        public async Task<IActionResult> GetEventsByMonthAndYear(string year, string month)
        {
            ApiClient<List<Event>> client = new ApiClient<List<Event>>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/GetEventsByMonthAndYear";
            client.AddParameter("year", year);
            client.AddParameter("month", month);

            List<Event> events = await client.GetAsync();
            if (events == null)
            {
                return StatusCode(404);
            }
            return Ok(events);
        }

        private async Task<string> SendReader()
        {
            return null;
        }
    }
}
