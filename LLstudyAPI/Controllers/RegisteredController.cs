using LLStudy_Models.Models;
using LLstudyWS.ORM.Repositorys;
using Microsoft.AspNetCore.Mvc;
using LLStudy_Models.ViewModels;
using LLStudy_Models.ViewModels.Registerd;
using System.Text.Json;
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
                viewModel.CartBooks = new List<CartBookViewModel>();
                viewModel.User = this.repositoryUOW.RegisteredRepository.GetByID(registeredID);
                viewModel.CartBooks = this.repositoryUOW.BookRepository.GetShoppingCartBooks(registeredID);
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
                viewOrdersModel.Orders = new List<ViewOrderDetailsModel>();
                List<Order> orders = this.repositoryUOW.OrderRepository.GetUserOrders(registeredID);
                foreach (Order order in orders) {
                    viewOrdersModel.Orders.Add(new ViewOrderDetailsModel() { Order = order, Books = this.repositoryUOW.OrderRepository.GetOrderBooks(order.OrderID)});
                }

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

        //[HttpPost]

        //public bool AppendToCart(Shopping_Cart record)
        //{
        //    try
        //    {
        //        this.repositoryUOW.HelperOledb.OpenConnection();

        //        return this.repositoryUOW.ShoppingCartRepository.Create(record);

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        return false;
        //    }
        //    finally
        //    {
        //        this.repositoryUOW.HelperOledb.CloseConnection();
        //    }
        //}
        [HttpPost]
        public bool AddToCart(Shopping_Cart record)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                int exist = this.repositoryUOW.ShoppingCartRepository.CheckIfExist(record.BookID, record.RegisteredID);
                Console.WriteLine(@$"exist: {exist}");
                if (exist == 1)
                {
                    return this.repositoryUOW.ShoppingCartRepository.AppendToCart(record.BookID, record.RegisteredID);
                }
                else if (exist == -1)
                    return false;
                record.CountBooks = 1;
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
                //viewModel.Registered = this.repositoryUOW.RegisteredRepository.GetByID(viewModel.Order.RegisteredID);
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

        public bool RemoveAllBooksFromCart(Shopping_Cart record)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();

                
                return this.repositoryUOW.ShoppingCartRepository.RemoveBookForUser(record.BookID, record.RegisteredID);
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
        public bool RemoveFromCart(Shopping_Cart record)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();

                if (this.repositoryUOW.ShoppingCartRepository.CountBookForUser(record.BookID,record.RegisteredID) > 1)
                {
                    return this.repositoryUOW.ShoppingCartRepository.RemoveOneBookForUuser(record.BookID,record.RegisteredID);
                }
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

        [HttpPost]
        public bool ChangeImage([FromForm] string model, [FromForm] IFormFile file)
        {
            try
            {
                JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
                jsonSerializerOptions.PropertyNameCaseInsensitive = true;
                Registered modelReg = JsonSerializer.Deserialize<Registered>(model, jsonSerializerOptions);
                //if (!HttpContext.Request.Form.Files.Any())
                //    return false;
                //IFormFile image = HttpContext.Request.Form.Files[0];
                if (file == null || file.Length == 0)
                    throw new Exception("Empty file");

                string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName,"wwwroot","Images","RegisteredProfileImages");
                string ext = Path.GetExtension(file.FileName);
                if (string.IsNullOrEmpty(ext))
                {
                    ext = file.ContentType switch
                    {
                        "image/jpeg" => ".jpg",
                        "image/png" => ".png",
                        "image/gif" => ".gif",
                        _ => ".bin"
                    };
                }

                string fileName = $"User{modelReg.RegisteredID}{ext}";

                path = Path.Combine(path, fileName);
                Console.WriteLine("********************************" + path);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                if(modelReg.ImagePath == "None")
                {
                    modelReg = this.repositoryUOW.RegisteredRepository.GetByID(modelReg.RegisteredID);
                    modelReg.ImagePath = fileName;
                    this.repositoryUOW.RegisteredRepository.Update(modelReg, exludes : new List<string> {"Password","Role","RegisteredID","RegisteredSalt" });
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            
            
            
        }
    }
}
