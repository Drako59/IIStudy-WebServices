using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using LLStudy_Models.ViewModels.Registerd;
using LLstudyWS.ORM.Repositorys;
using System.IO;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

namespace LLstudyWS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RegisteredController : ControllerBase
    {
        RepositoryUOW repositoryUOW;
        string RegisteredsImagePath = Path.Combine(Directory.GetCurrentDirectory()!, "wwwroot", "Images", "RegisteredImages");
        readonly string BooksPdfPath = Path.Combine(Directory.GetCurrentDirectory()!, "wwwroot", "Files", "BooksFiles");
        readonly string ExamsPdfPath = Path.Combine(Directory.GetCurrentDirectory()!, "wwwroot", "Files", "ExamsFiles");
        readonly string SolutionsPdfPath = Path.Combine(Directory.GetCurrentDirectory()!, "wwwroot", "Files", "SolutionsFiles");


        public RegisteredController() {
            this.repositoryUOW = new RepositoryUOW();
        }


        [HttpGet]
        public ViewRegisterdBookCatalogModel GetBooks(string registeredID,string? subjectID = null, string? author_name = null, string? search = null, string? book_name = null, string? price_min = null, string? price_max = null, string? type = null)
        {

            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();


                ViewRegisterdBookCatalogModel viewModel = new ViewRegisterdBookCatalogModel();
                List<Book> books = new List<Book>();

                if (search == null && subjectID == null && author_name == null && book_name == null && price_min == null && price_max == null && type == null)
                    books =  this.repositoryUOW.BookRepository.GetExistBooks();
                //return this.repositoryUOW.BookRepository.GetAll();


                if (search != null)
                    books.AddRange(this.repositoryUOW.BookRepository.GetByName(search));

                viewModel.books = books;
                viewModel.OwnedOnlineBooksIDs = this.repositoryUOW.BookRepository.GetOwnedOnlineBooksIDsForUser(registeredID);
                viewModel.OnlineBooksInShoppingCartIDs = this.repositoryUOW.ShoppingCartRepository.GetRegCartOnlieBooksIDs(registeredID);
    




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
        public ViewOwnedBooksModel GetUserBooks(string registeredID)
        {
            try
            {
                ViewOwnedBooksModel model = new ViewOwnedBooksModel();
                this.repositoryUOW.HelperOledb.OpenConnection();
                List<ViewOwnedBook> books = new List<ViewOwnedBook>();
                books = this.repositoryUOW.BookRepository.GetUserNameBooks(registeredID);
                model.Books = books;
                model.User = this.repositoryUOW.RegisteredRepository.GetByID(registeredID,exludes : new List<string>() { "Password", "RegisteredSalt"});
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
        public IActionResult GetShoppingCart(string registeredID)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();

                ViewShoppingCartModel viewModel = new ViewShoppingCartModel();
                viewModel.CartBooks = new List<CartBookViewModel>();
                viewModel.User = this.repositoryUOW.RegisteredRepository.GetByID(registeredID);
                viewModel.CartBooks = this.repositoryUOW.BookRepository.GetShoppingCartBooks(registeredID);
                return Ok(viewModel);

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
        public bool Pay(Order order) 
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                this.repositoryUOW.HelperOledb.OpenTransaction();
                if (this.repositoryUOW.ShoppingCartRepository.CartIsEmpty(order.RegisteredID)) 
                    return false;

                //Create the order
                if (!(this.repositoryUOW.ShoppingCartRepository.GetRegCartPhysicalBooksIDs(order.RegisteredID).Any()))
                    order.DeliveryStatus = (int)OrderStatus.Delivered;

                order.Total_price = this.repositoryUOW.ShoppingCartRepository.GetTotalPriceForUser(order.RegisteredID);
                this.repositoryUOW.OrderRepository.Create(order);


                //Add the relations of books and order
                string orderID = this.repositoryUOW.OrderRepository.GetLastID();

                this.repositoryUOW.OrderRepository.AddRealationOfBooksAndOrder(orderID, order.RegisteredID);

                
                //remove the books that got bought from shopping cart
                this.repositoryUOW.ShoppingCartRepository.RemoveAllBooksForUser(order.RegisteredID);
                
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
                //Console.WriteLine(@$"exist: {exist}");
                if (this.repositoryUOW.BookRepository.IsOwnedOnlineBook(record.BookID, record.RegisteredID))
                    return false;
                if (exist == 1)
                {
                    if (this.repositoryUOW.BookRepository.IsOnlineBook(record.BookID) )
                        return false;
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
        public bool AddReview(Review model)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                model.ReviewID = this.repositoryUOW.ReviewRepository.GetUserReviewOnBook(model.RegisteredID, model.BookID);
                if (model.ReviewID != "0")
                    return this.repositoryUOW.ReviewRepository.Update(model);
                return this.repositoryUOW.ReviewRepository.Create(model);

                
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
                    return this.repositoryUOW.ShoppingCartRepository.RemoveOneBookForUser(record.BookID,record.RegisteredID);
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
        public bool ChangeImage([FromForm] string model)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                if(HttpContext.Request.Form.Count == 0)
                {
                    throw new Exception("A file was not found.");
                    return false;
                }
                IFormFile file = HttpContext.Request.Form.Files[0];
                JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
                jsonSerializerOptions.PropertyNameCaseInsensitive = true;
                Registered modelReg = JsonSerializer.Deserialize<Registered>(model, jsonSerializerOptions);
                

                //if(modelReg.ImagePath == "None")
                //{
                modelReg = this.repositoryUOW.RegisteredRepository.GetByID(modelReg.RegisteredID);
                modelReg.ImagePath = this.repositoryUOW.RegisteredRepository.ChangeImage(file,modelReg.RegisteredID);
                this.repositoryUOW.RegisteredRepository.Update(modelReg, exludes : new List<string> {"Password","Role","RegisteredID","RegisteredSalt" });
                //}

                return true;
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
        public IActionResult GetProfileImage(string registeredID)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                
                string AbsoultePath;
                Registered reg = this.repositoryUOW.RegisteredRepository.GetByID(registeredID);
                if(reg.ImagePath != null && reg.ImagePath != "None")
                {
                    AbsoultePath = Path.Combine(this.RegisteredsImagePath, reg.ImagePath);

                }
                else
                {
                    return NoContent();
                    AbsoultePath = Path.Combine(this.RegisteredsImagePath, "zoro2.jpg");
                }

                //FileStream stream = System.IO.File.OpenRead(AbsoultePath);
                ////string contentType = "application/octet-stream";
                //string ext = Path.GetExtension(AbsoultePath).ToLowerInvariant();
                //string contentType = ext switch
                //{
                //    ".jpg" or ".jpeg" => "image/jpeg",
                //    ".png" => "image/png",
                //    ".gif" => "image/gif",
                //    _ => "application/octet-stream"
                //};
                //IFormFile formFile = new FormFile(stream, 0, stream.Length, null, reg.ImagePath)
                //{
                //    Headers = new HeaderDictionary(),
                //    ContentType = contentType
                //};
                var (stream, contentType) = this.repositoryUOW.RegisteredRepository.GetImage(AbsoultePath);

                return File(stream, contentType);

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return  StatusCode(500, "Error loading image");
            }
            finally
            {
                this.repositoryUOW.HelperOledb.CloseConnection();
            }
        }

        [HttpGet]
        public IActionResult GetBookFile(string bookID, string registeredID)
        {
            try
            {
                string AbsoultePath;
                this.repositoryUOW.HelperOledb.OpenConnection();
                Book book = this.repositoryUOW.BookRepository.GetByID(bookID);
                AbsoultePath = Path.Combine(this.BooksPdfPath, book.Pdf_url_book);
                if (!(book.Pdf_url_book != null && book.Pdf_url_book.ToLower() != "none" && System.IO.File.Exists(AbsoultePath) && this.repositoryUOW.OrderRepository.CheckIfBookExistForUser(bookID,registeredID)))
                    return StatusCode(404);


                var (stream, contentType) = this.repositoryUOW.BookRepository.GetPdf(AbsoultePath);

                return File(stream, contentType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500);
            }
            finally
            {
                this.repositoryUOW.HelperOledb.CloseConnection();
            }
        }

        [HttpGet]
        public IActionResult GetExamFile(string examID)
        {
            try
            {
                string AbsoultePath;
                this.repositoryUOW.HelperOledb.OpenConnection();
                Exam exam = this.repositoryUOW.ExamRepository.GetByID(examID);
                AbsoultePath =  Path.Combine(this.ExamsPdfPath, exam.File_path_url);
                ;
                if (!(exam != null && exam.File_path_url != null && exam.File_path_url.ToLower() != "none" && System.IO.File.Exists(AbsoultePath) ))
                    return StatusCode(404);


                var (stream, contentType) = this.repositoryUOW.ExamRepository.GetPdf(AbsoultePath);

                return File(stream, contentType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500);
            }
            finally
            {
                this.repositoryUOW.HelperOledb.CloseConnection();
            }
        }

        [HttpGet]
        public IActionResult GetSolutionFile(string solutionID)
        {
            try
            {
                string AbsoultePath;
                this.repositoryUOW.HelperOledb.OpenConnection();
                Solution solution = this.repositoryUOW.SolutionRepository.GetByID(solutionID);
                AbsoultePath = Path.Combine(this.SolutionsPdfPath, solution.File_path_url);
                
                if (!(solution != null && solution.File_path_url != null && solution.File_path_url.ToLower() != "none" && System.IO.File.Exists(AbsoultePath)))
                    return StatusCode(404);

                
                var (stream, contentType) = this.repositoryUOW.SolutionRepository.GetPdf(AbsoultePath);

                return File(stream, contentType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500);
            }
            finally
            {
                this.repositoryUOW.HelperOledb.CloseConnection();
            }
        }

        [HttpGet]
        public ViewRegisteredBookPreviewModel GetBookFullView(string registeredID ,string bookID)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();

                ViewRegisteredBookPreviewModel viewModel = new ViewRegisteredBookPreviewModel();

                viewModel.BookViewModel = this.repositoryUOW.BookRepository.GetFullBook(bookID);
                viewModel.IsOwned = this.repositoryUOW.BookRepository.IsOwnedOnlineBook(bookID,registeredID);
                viewModel.IsInShoppingCart = this.repositoryUOW.ShoppingCartRepository.IsOnlineBookInCart(registeredID,bookID);
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
    }
}
