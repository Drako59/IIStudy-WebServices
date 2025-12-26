using IIstudyWSClient;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using LLStudy_Models.ViewModels.Registerd;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IIStudyWebApp.Controllers
{
    public class RegisteredController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> RegisteredHomePage(string registeredID)
        {
            ApiClient<Registered> client = new ApiClient<Registered>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/prfoile";
            client.AddParameter("registeredID", registeredID);

            Registered registered = await client.GetAsync();
            return View(registered);
        }
        [HttpGet]
        public async Task<IActionResult> Profile(string registeredID)
        {
            ApiClient<Registered> client = new ApiClient<Registered>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/prfoile";
            client.AddParameter("registeredID", registeredID);

            Registered registered = await client.GetAsync();
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
        public async Task<IActionResult> ViewShoppingCart(string registeredID)
        {
            ApiClient<ViewShoppingCartModel> client = new ApiClient<ViewShoppingCartModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/GetShoppingCart";
            client.AddParameter("registeredID", registeredID);

            ViewShoppingCartModel shoppingCart = await client.GetAsync();
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
        public async Task<IActionResult> AddToCart(Shopping_Cart record)
        {
            ApiClient<Shopping_Cart> client = new ApiClient<Shopping_Cart>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Registered/AddToCart";


            bool success = client.PostAsync(record).Result;

            if (!success)
                return View("Failed to pay.");
            return View("Payment suceed.");
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
    }
}
