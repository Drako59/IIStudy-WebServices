using IIstudyWSClient;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using LLStudy_Models.ViewModels.Guest;
using LLStudy_Models.ViewModels.Registerd;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using static NuGet.Packaging.PackagingConstants;

namespace IIStudyWebApp.Controllers
{
    public class RegisteredController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> RegisteredHomePage()
        {
            string registeredID = HttpContext.Session.GetString("RegisteredID");
            //RegisteredViewModel registeredInfo = new RegisteredViewModel();
            //registeredInfo.registered = GetRegisteredDeatils().Result;
            if (registeredID == null)
            {
                return RedirectToAction("ViewBookPreview", "Guest");
            }
            ApiClient<Registered> client = new ApiClient<Registered>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/profile";
            client.AddParameter("registeredID", registeredID);
            Registered registered = await client.GetAsync();
            ViewData["Registered"] = registered;

            return View(registered);
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            Registered registered = await GetRegisteredDeatils();
            if (registered == null)
            {
                return RedirectToAction("ViewSignInPage", "Guest");
            }
            //RegisteredViewModel registeredInfo = new RegisteredViewModel();
            //registeredInfo.registered = GetRegisteredDeatils().Result;

            if (registered == null) //registeredInfo.registered;
            {
                return RedirectToAction("HomePage", "Guest");
            }
            ViewData["Registered"] = registered;

            return View(registered);
        }

        [HttpGet]
        public async Task<IActionResult> ViewOwnedBooks(string registeredID)
        {
            ApiClient<ViewOwnedBooksModel> client = new ApiClient<ViewOwnedBooksModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/GetUserBooks";
            client.AddParameter("registeredID", registeredID);

            ViewOwnedBooksModel books = await client.GetAsync();

            if (books == null)
                return StatusCode(500);

            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> ViewShoppingCart()
        {

            string registeredID = HttpContext.Session.GetString("RegisteredID");

            //RegisteredViewModel registeredInfo = new RegisteredViewModel();
            //registeredInfo.registered = GetRegisteredDeatils().Result;

            if (string.IsNullOrWhiteSpace(registeredID))
            {
                // The is not a connected user.
                return RedirectToAction("HomePage", "Guest");
            }
            ApiClient<ViewShoppingCartModel> client = new ApiClient<ViewShoppingCartModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/GetShoppingCart";
            
            client.AddParameter("registeredID", registeredID);

            ViewShoppingCartModel shoppingCart = await client.GetAsync();

            ViewData["Registered"] = shoppingCart.User;

            if (shoppingCart == null)
            {
                return StatusCode(500);
            }

            return View(shoppingCart);
        }
        [HttpGet]
        public async Task<IActionResult> ViewUserOrders()
        {
            string registeredID = HttpContext.Session.GetString("RegisteredID");
            ApiClient<ViewOrdersModel> client = new ApiClient<ViewOrdersModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/GetUserOrders";
            client.AddParameter("registeredID", registeredID);



            ViewOrdersModel orders = await client.GetAsync();
            ViewData["Registered"] = orders.User;

            if(orders == null)
            {
                return StatusCode(500);
            }

            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> ViewOrderDetails(string orderID)
        {
            ApiClient<Order> client = new ApiClient<Order>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/GetOrderDetails";
            client.AddParameter("orderID", orderID);

            Order order = await client.GetAsync();
            if(order == null)
            {
                return StatusCode(404);
            }
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> ViewOrderFullDetails(string orderID)
        {
            ApiClient<ViewOrderDetailsModel> client = new ApiClient<ViewOrderDetailsModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/GetOrderFullDetails";
            client.AddParameter("orderID", orderID);

            ViewOrderDetailsModel orderDetailsModel = await client.GetAsync();

            if(orderDetailsModel == null)
            {
                return StatusCode(404);
            }

            return View(orderDetailsModel);
        }

        [HttpPost]
        public async Task<IActionResult> Pay(PaymentViewModel payment)
        {
            ApiClient<PaymentViewModel> client = new ApiClient<PaymentViewModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/Pay";


            bool success = await client.PostAsync(payment);

            if (!success)
                return View("Failed to pay.");
            return View("Payment succeed.");
        }

        [HttpGet]
        public async Task<IActionResult> RemoveFromCart(string BookID)
        {
            string registeredID = HttpContext.Session.GetString("RegisteredID");
            if (string.IsNullOrWhiteSpace(registeredID))
            {
                // The is not a connected user.
                return RedirectToAction("viewSignInPage", "Guest");
            }
            
            Shopping_Cart record = new Shopping_Cart();
            record.BookID = BookID;
            record.RegisteredID = registeredID;
            record.CountBooks = 1;

            ApiClient<Shopping_Cart> client = new ApiClient<Shopping_Cart>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/RemoveFromCart";


            bool success = await client.PostAsync(record);


            return Json(new { success = success });
            return RedirectToAction("ViewShoppingCart", "Registered"); //difrrent approach 
        }

        public async Task<IActionResult> RemoveAllBooksFromCart(string BookID)
        {
            string registeredID = HttpContext.Session.GetString("RegisteredID");
            if (string.IsNullOrWhiteSpace(registeredID))
            {
                // The is not a connected user.
                return RedirectToAction("viewSignInPage", "Guest");
            }

            Shopping_Cart record = new Shopping_Cart();
            record.BookID = BookID;
            record.RegisteredID = registeredID;
            record.CountBooks = 1;

            ApiClient<Shopping_Cart> client = new ApiClient<Shopping_Cart>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/RemoveAllBooksFromCart";


            bool success =await client.PostAsync(record);


            return Json(new { success = success });
            return RedirectToAction("ViewShoppingCart", "Registered"); //difrrent approach 
        }

        //RemoveAllBooksFromCart

        [HttpGet]
        public async Task<IActionResult> AddToCart(string BookID)
        {
            string registeredID = HttpContext.Session.GetString("RegisteredID");
            //Console.WriteLine("Here");
            //Console.WriteLine("registeredID " + registeredID);
            if (string.IsNullOrWhiteSpace(registeredID))
            {
                // The is not a connected user.
                return RedirectToAction("viewSignInPage", "Guest");
            }
            Shopping_Cart record = new Shopping_Cart();
            record.BookID = BookID;
            record.RegisteredID = registeredID;
            record.CountBooks = 1;


            ApiClient<Shopping_Cart> client = new ApiClient<Shopping_Cart>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/AddToCart";


            ApiResultModel<bool> success = await client.PostAsyncRet<Shopping_Cart, bool>(record);

            if (!success.Success)
                return StatusCode(500);
            return  Json(new { success = success.Data });
        }

        public async Task<IActionResult> AddReview(Review record)
        {
            ApiClient<Review> client = new ApiClient<Review>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/AddReview";

            string registeredID = HttpContext.Session.GetString("RegisteredID");

            if (registeredID == null) { return RedirectToAction("viewSignInPage", "Guest"); }
            
            record.RegisteredID = registeredID;
            record.ReviewID = "0";
            bool success = await client.PostAsync(record);

            if (!success)
                return RedirectToAction("ViewBookPreview","Registered", new { bookID = record.BookID });
            return RedirectToAction("ViewBookPreview", "Registered", new { bookID = record.BookID });
        }

        public async Task<IActionResult> SignOut()
        {


            HttpContext.Session.Remove("RegisteredID");

            return RedirectToAction("HomePage","Guest");
            

            
        }

        [HttpGet]
        public async Task<IActionResult> ViewBookCatalog()
        {
            Registered registered = await GetRegisteredDeatils();
            //RegisteredViewModel registeredInfo = new RegisteredViewModel();
            //registeredInfo.registered = GetRegisteredDeatils().Result;

            if (registered == null)
            {
                return RedirectToAction("ViewBookPreview", "Guest");
            }
            
            ApiClient<ViewRegisterdBookCatalogModel> client = new ApiClient<ViewRegisterdBookCatalogModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/GetBooks";
            client.AddParameter("registeredID", registered.RegisteredID);
            ViewRegisterdBookCatalogModel books = await client.GetAsync();
            ViewData["Registered"] = registered;
            return View(books);
        }

        public async Task<IActionResult> ViewBookPreview(string bookID)
        {
            Registered registered = await GetRegisteredDeatils();

            //RegisteredViewModel registeredInfo = new RegisteredViewModel();
            //registeredInfo.registered = GetRegisteredDeatils().Result;

            if (registered == null)
            {
                return RedirectToAction("ViewBookPreview", "Guest");
            }
            ApiClient<ViewRegisteredBookPreviewModel> client = new ApiClient<ViewRegisteredBookPreviewModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/GetBookFullView";
            client.AddParameter("bookID", bookID);
            client.AddParameter("registeredID", registered.RegisteredID);

            ViewRegisteredBookPreviewModel bookView = await client.GetAsync();
            ViewData["Registered"] = registered;
            
            if(bookView == null)
            {
                return StatusCode(500);
            }

            return View(bookView);
        }

        public async Task<IActionResult> ViewExams(string year = null, string subjectID = null, int pages = 0) //Not in use => remove
        {
            ApiClient<ViewExamsModel> client = new ApiClient<ViewExamsModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/GetExams";
            client.AddParameter("year", year);
            client.AddParameter("subjectID", subjectID);
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
            if(calender == null)
            {
                calender = new List<Event>();
            }
            return View(calender);
        }

        private async Task<Registered> GetRegisteredDeatils()
        {
            
            string registeredID = HttpContext.Session.GetString("RegisteredID");
            if (string.IsNullOrWhiteSpace(registeredID))
            {
                // The is not a connected user.

                return null;
            }
            ApiClient<Registered> client = new ApiClient<Registered>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/profile";
            client.AddParameter("registeredID", registeredID);
            Registered registered = await client.GetAsync();
            //Console.WriteLine(@$"{registered.UserName} {registeredID}");

            return registered;

        }

        [HttpPost]
        public async Task<IActionResult> ChangeProfileImage([FromForm] IFormFile image)
        {
            IFormFile formFile = image;
            //if (!HttpContext.Request.Form.Files.Any())
            //{
            //    await Console.Out.WriteLineAsync( "Here, there is not any file");
            //    return RedirectToAction( "Profile", "Registered");   
            //}

            //formFile = HttpContext.Request.Form.Files[0];


            if (formFile == null || formFile.Length == 0)
            {
                //await Console.Out.WriteLineAsync("Here, there is not any file");

                return RedirectToAction("Profile", "Registered");
                
            }
            Registered reg = await GetRegisteredDeatils();
            if(reg == null)
            {
                return RedirectToAction( "ViewSignInPage", "Guest");
            }

            ApiClient<Registered> client = new ApiClient<Registered>();

            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/ChangeImage";
            
            bool success = await client.PostAsync(reg, new List<(Stream,string)>() { 
                (formFile.OpenReadStream(),formFile.FileName) 
            });

            //await Console.Out.WriteLineAsync("here*************************************" + success.ToString());

            return RedirectToAction("Profile", "Registered");






        }

        [HttpGet]
        public async Task<IActionResult> GetProfileImage(string registeredID)
        {
            ApiClient<Registered> client = new ApiClient<Registered>();

            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/GetProfileImage";
            client.AddParameter("registeredID", registeredID);
            ApiFileResultModel file = await client.GetAsyncFile();

            if(file == null)
            {
                return StatusCode(404); 
            }
            
            return File(file.Bytes, file.ContentType);
        }

        [HttpPost]

        public async Task<IActionResult> PayShoppingCart(Order order )
        {

            order.OrderID = "0";
            DateTime now = DateTime.Now;
            string customFormat = now.ToString("yyyy-MM-dd");

            order.Date = customFormat;
            //order.Delivered = false;
            order.DeliveryStatus = (int)OrderStatus.Pending;
            order.RegisteredID = HttpContext.Session.GetString("RegisteredID");
            if (order.RegisteredID == null)
                return RedirectToAction("viewSignInPage", "Guest");

            ApiClient<Order> client = new ApiClient<Order>();

            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/Pay";
            
            ApiResultModel<PaymentResult> status = await client.PostAsyncRet<Order, PaymentResult>(order);
            if(status.Success && status.Data.Success)
                return RedirectToAction("RegisteredHomePage", "Registered");
            if (status.Success && !status.Data.Success)
                return RedirectToAction("ViewShoppingCart","Registered");
            return RedirectToAction("PaymentPage", "Registered", order);

            //if (status.Success && status.Data)
            //    return View();
            //return View();

        }

        [HttpGet]
        public async Task<IActionResult> PaymentPage()
        {
            Registered registered = await GetRegisteredDeatils();
            if (registered == null)
                return RedirectToAction("viewSignInPage", "Guest");
            ViewData["Registered"] = registered;

            ApiClient<Order> client = new ApiClient<Order>();

            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/Pay";

            //string totalPrice = client.GetAsync();

            //To continue

            return View();
            
        }

        [HttpGet]
        public async Task<IActionResult> GetBookFile(string bookID)
        {
            ApiClient<bool> client = new ApiClient<bool>();

            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/GetBookFile";
            client.AddParameter("bookID", bookID);
            string regID = HttpContext.Session.GetString("RegisteredID");
            if (regID == null)
                return RedirectToAction("viewSignInPage", "Guest");
            client.AddParameter("registeredID", regID);

            ApiFileResultModel file = await client.GetAsyncFile();

           
            if (file == null)
            {
                return StatusCode(404);
            }
            return File(file.Bytes, file.ContentType);
        }

        [HttpGet]
        public async Task<IActionResult> ViewMyLibary()
        {
            ApiClient<ViewOwnedBooksModel> client = new ApiClient<ViewOwnedBooksModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/GetUserBooks";
            string regID = HttpContext.Session.GetString("RegisteredID");
            if (regID == null)
                return RedirectToAction("viewSignInPage", "Guest");
            client.AddParameter("registeredID", regID );



            ViewOwnedBooksModel viewOwnedBooksModels = await client.GetAsync();
            if (viewOwnedBooksModels == null ||viewOwnedBooksModels.User == null)
                return StatusCode(500);
            ViewData["Registered"] = viewOwnedBooksModels.User;

            return View(viewOwnedBooksModels);
        }

        [HttpGet]
        public async Task<IActionResult> ViewCalendar()
        {
            Registered registered = await this.GetRegisteredDeatils();
            if (registered == null)
            {
                return RedirectToAction("ViewCalendar", "Guest");
            }
            ViewData["Registered"] = registered;

            return View();
        }


        [HttpGet]

        public async Task<IActionResult> ViewExamsPage()
        {
            Registered reg = await this.GetRegisteredDeatils();
            if (reg == null)
            {
                return RedirectToAction("ViewExamsPage", "Guest");
            }
            ViewData["Registered"] = reg;
            ApiClient<Dictionary<string, string>> client = new ApiClient<Dictionary<string, string>>();

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
            Registered reg = await this.GetRegisteredDeatils();
            if(reg == null)
            {
                return RedirectToAction("ViewExamYearsPage", "Guest");
            }

            ViewData["Registered"] = reg;


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
            Registered reg = await this.GetRegisteredDeatils();
            if (reg == null)
            {
                return RedirectToAction("ViewExamYearsPage", "Guest");
            }

            ViewData["Registered"] = reg;


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
        public async Task<IActionResult> ViewSolutionsByExam(string examID)
        {
            Registered reg = await this.GetRegisteredDeatils();
            if (reg == null)
            {
                return RedirectToAction("ViewExamYearsPage", "Guest");
            }

            ViewData["Registered"] = reg;

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


    }
}
