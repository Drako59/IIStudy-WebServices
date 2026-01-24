using LLStudy_Models.Models;
using LLstudyWS.ORM.Repositorys;
using Microsoft.AspNetCore.Mvc;
using LLStudy_Models.ViewModels;
using LLStudy_Models.ViewModels.Registerd;
namespace LLstudyWS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RegisteredController : ControllerBase
    {
        RepositoryUOW repositoryUOW;

        public RegisteredController() {
            this.repositoryUOW = new RepositoryUOW();
        }
        

        [HttpGet]
        public ViewOwnedBooksModel GetUserBooks(string registeredID)
        {
            try
            {
                ViewOwnedBooksModel model = new ViewOwnedBooksModel();
                this.repositoryUOW.HelperOledb.OpenConnection();
                List<Book> books = new List<Book>();
                books = this.repositoryUOW.BookRepository.GetUserNameBooks(registeredID);
                model.Books = books;
                model.User = this.repositoryUOW.RegisteredRepository.GetByID(registeredID);
                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            finally
            {
                this.repositoryUOW.HelperOledb.CloseConnection();
            }
        }

        

        [HttpGet]
        public Registered profile(string registeredID)
        {
            try
            {
                Registered reg;
                this.repositoryUOW.HelperOledb.OpenConnection();
                reg = this.repositoryUOW.RegisteredRepository.GetByID(registeredID, new List<string>() {"HasErrors", "IsValid" } );
                return reg; 
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            finally
            {
                this.repositoryUOW.HelperOledb.CloseConnection();
            }
        }


        [HttpGet]
        public ViewShoppingCartModel GetShoppingCart(string registeredID)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();

                ViewShoppingCartModel viewModel = new ViewShoppingCartModel();

                viewModel.User = this.repositoryUOW.RegisteredRepository.GetByID(registeredID);
                viewModel.Books = this.repositoryUOW.BookRepository.GetShoppingCartBooks(registeredID);
                return viewModel;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            finally 
            {
                this.repositoryUOW.HelperOledb.CloseConnection();
            }
            
        }

        [HttpGet]
        public ViewOrdersModel GetUserOrders(string registeredID)
        {
            try
            {
                
                this.repositoryUOW.HelperOledb.OpenConnection();
                ViewOrdersModel viewOrdersModel = new ViewOrdersModel();
                viewOrdersModel.Orders = this.repositoryUOW.OrderRepository.GetUserOrders(registeredID);
                viewOrdersModel.User = this.repositoryUOW.RegisteredRepository.GetByID(registeredID);
                return viewOrdersModel;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            finally
            {
                this.repositoryUOW.HelperOledb.CloseConnection();
            }
        }

        [HttpGet]

        public Order GetOrderDetails(string orderID)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                return this.repositoryUOW.OrderRepository.GetByID(orderID, new List<string>() { "IsValid", "HasErrors"});
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            finally {
                this.repositoryUOW.HelperOledb.CloseConnection();
            }
        }


        //return ID
        [HttpPost]
        public bool Pay( PaymentViewModel payment)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                this.repositoryUOW.HelperOledb.OpenTransaction();
                this.repositoryUOW.OrderRepository.Create(payment.Order);
                string orderID = this.repositoryUOW.OrderRepository.GetLastID() ;
                Console.WriteLine("OrderID-> "+ orderID);
                foreach (string bookID in payment.BooksID)
                {
                    Console.Write(bookID + "->");
                    this.repositoryUOW.OrderRepository.AddRealationOfBooksAndOrder(orderID, bookID);
                }
                Console.WriteLine(  );
                this.repositoryUOW.HelperOledb.Commit();

                return true ;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.repositoryUOW.HelperOledb.RollBack();
                return false;
            }
            finally
            {

                this.repositoryUOW.HelperOledb.CloseConnection();
            }
        }

        [HttpPost]
        public bool AddToCart(Shopping_Cart record)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                
                return this.repositoryUOW.ShoppingCartRepository.Create(record);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                this.repositoryUOW.HelperOledb.CloseConnection();
            }
        }

        [HttpPost]
        public bool AddReview(Review review)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();

                return this.repositoryUOW.ReviewRepository.Create(review);

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            finally 
            {
                this.repositoryUOW.HelperOledb.CloseConnection();
            }

        }

        [HttpGet]
        public ViewOrderDetailsModel GetOrderFullDetails(string orderID)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                ViewOrderDetailsModel viewModel = new ViewOrderDetailsModel();
                viewModel.Order = this.repositoryUOW.OrderRepository.GetByID(orderID);
                viewModel.Books = this.repositoryUOW.OrderRepository.GetOrderBooks(orderID);
                viewModel.Registered = this.repositoryUOW.RegisteredRepository.GetByID(viewModel.Order.RegisteredID);
                return viewModel;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            finally
            {
                this.repositoryUOW.HelperOledb.CloseConnection();
            }
        }

        [HttpPost]
        public bool RemoveFromCart(Shopping_Cart record)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                return this.repositoryUOW.ShoppingCartRepository.RemoveBookForUser(record.BookID,record.RegisteredID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                this.repositoryUOW.HelperOledb.CloseConnection();
            }
        }
    }
}
