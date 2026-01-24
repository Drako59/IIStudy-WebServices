using IIstudyWSClient;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using LLStudy_Models.ViewModels.Registerd;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

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
            Registered registered = GetRegisteredDeatils().Result;
            if (registered == null)
            {
                return RedirectToAction("ViewBookPreview", "Guest");
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
            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> ViewShoppingCart()
        {

            string registeredID = HttpContext.Session.GetString("RegisteredID");
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
            return View(shoppingCart);
        }
        [HttpGet]
        public async Task<IActionResult> ViewUserOrders(string registeredID)
        {
            ApiClient<ViewOrdersModel> client = new ApiClient<ViewOrdersModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/GetUserOrders";
            client.AddParameter("registeredID", registeredID);

            ViewOrdersModel orders = await client.GetAsync();
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


            bool success = client.PostAsync(payment).Result;

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

            ApiClient<Shopping_Cart> client = new ApiClient<Shopping_Cart>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/RemoveFromCart";


            bool success = client.PostAsync(record).Result;


            return Json(new { success = success });
            return RedirectToAction("ViewShoppingCart", "Registered"); //difrrent approach 
        }

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

            ApiClient<Shopping_Cart> client = new ApiClient<Shopping_Cart>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/AddToCart";


            bool success = client.PostAsync(record).Result;

            
            return  Json(new { success = success });
        }

        public async Task<IActionResult> AddReview(Review record)
        {
            ApiClient<Review> client = new ApiClient<Review>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/AddReview";


            bool success = client.PostAsync(record).Result;

            if (!success)
                return View("Failed to add.");
            return View("Review added.");
        }

        public async Task<IActionResult> SignOut()
        {


            HttpContext.Session.Remove("RegisteredID");

            return RedirectToAction("HomePage","Guest");
            

            
        }

        [HttpGet]
        public async Task<IActionResult> ViewBookCatalog()
        {
            Registered registered = GetRegisteredDeatils().Result;
            if (registered == null)
            {
                return RedirectToAction("ViewBookPreview", "Guest");
            }

            ApiClient<List<Book>> client = new ApiClient<List<Book>>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/GetBooks";
            List<Book> books = await client.GetAsync();
            ViewData["Registered"] = registered;
            return View(books);
        }

        public async Task<IActionResult> ViewBookPreview(string bookID)
        {
            Registered registered = GetRegisteredDeatils().Result;
            if (registered == null)
            {
                return RedirectToAction("ViewBookPreview", "Guest");
            }
            ApiClient<ViewBookViewModel> client = new ApiClient<ViewBookViewModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/GetBookFullView";
            client.AddParameter("bookID", bookID);

            ViewBookViewModel bookView = await client.GetAsync();
            ViewData["Registered"] = registered;
            
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
            Console.WriteLine(@$"{registered.UserName} {registeredID}");

            return registered;

        }
    }
}
