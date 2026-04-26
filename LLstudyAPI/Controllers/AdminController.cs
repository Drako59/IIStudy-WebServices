using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using LLstudyWS.ORM.Repositorys;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection;
using System.Text.Json;

namespace LLstudyWS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
        public class AdminController : ControllerBase
        {
            RepositoryUOW repositoryUOW;
            public AdminController() {
                this.repositoryUOW = new RepositoryUOW();
            }


            [HttpPost]
            public bool AddEvent(Event event_var)
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();

                    return this.repositoryUOW.EventRepository.Create(event_var);

                }
                catch(Exception ex)
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
         
            public bool BanUser([FromBody] Registered reg)
            {
                try{
                    this.repositoryUOW.HelperOledb.OpenConnection();
                    reg.IsBanned = true;
                    if (reg.RegisteredID == "39")
                        return false;
                    return this.repositoryUOW.RegisteredRepository.Update(reg, exludes : new List<string>() { "RegisteredSalt", "PhoneNumber","ImagePath", "Role","Birth", "Email","Password","UserName"});
                }
                catch(Exception ex)
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

            public bool UnBanUser([FromBody] Registered reg)
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();
                    reg.IsBanned = false;
                    return this.repositoryUOW.RegisteredRepository.Update(reg, exludes: new List<string>() { "RegisteredSalt", "PhoneNumber", "ImagePath", "Role", "Birth", "Email", "Password", "UserName" });
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
            public bool RemoveBook([FromBody]Book book)
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();
                    bool flag = false;
                    this.repositoryUOW.HelperOledb.OpenTransaction();
                    book.IsDeleted = true;
                    flag = this.repositoryUOW.ShoppingCartRepository.RemoveBook(book.BookID) && this.repositoryUOW.BookRepository.Update(book, exludes: new List<string>() { "BookImagePath", "BookDetails", "BookAuthorImage", "Pdf_url_book", "Type", "SubjectID", "In_stock" , "Book_name", "Book_price", "Author_name" });

                    this.repositoryUOW.HelperOledb.Commit();
                    return flag;
                }
                catch (Exception ex) {
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
            public bool RestoreBook([FromBody] Book book)
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();
                    book.IsDeleted = false;
                    return  this.repositoryUOW.BookRepository.Update(book, exludes: new List<string>() { "BookImagePath", "BookDetails", "BookAuthorImage", "Pdf_url_book", "Type", "SubjectID", "In_stock", "Book_name", "Book_price", "Author_name" });
                
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
            public bool RemoveReview([FromBody] Review review)
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();

                    return this.repositoryUOW.ReviewRepository.Delete(review.ReviewID);

                }
                catch(Exception ex) 
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

            public bool RemoveEvent([FromBody] Event Event)
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();

                    return this.repositoryUOW.EventRepository.Delete(Event.EventID);

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
            public bool RemoveExam([FromBody] Exam exam)
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();
                    return this.repositoryUOW.ExamRepository.Delete(exam.ExamID);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false; ;
                }
                finally 
                {
                    this.repositoryUOW.HelperOledb.CloseConnection();
                }
            }

            [HttpPost]

            public bool RemoveSolution([FromBody] Solution solution)
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();
                    return this.repositoryUOW.SolutionRepository.Delete(solution.SolutionID);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false; ;
                }
                finally
                {
                    this.repositoryUOW.HelperOledb.CloseConnection();
                }
            }

            [HttpPost]

            public bool AddExam(Exam exam)
            {
                try 
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();

                    return this.repositoryUOW.ExamRepository.Create(exam);
                }
                catch(Exception ex)
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
            public bool AddSolution(Exam exam)
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();

                    return this.repositoryUOW.ExamRepository.Create(exam);
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
            public bool AddBook(Book book)
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();

                    return this.repositoryUOW.BookRepository.Create(book);
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

            public bool UpdateBook(Book book)
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();
                    List<string> exludes = new List<string>();
                    Type bookType = book.GetType();
                    PropertyInfo[] pros = bookType.GetProperties().Where(p => p.GetValue(book, null) == null).ToArray();
                    foreach(PropertyInfo pro  in pros)
                    {
                        exludes.Add(pro.Name);
                    }


                    return this.repositoryUOW.BookRepository.Update(book, exludes);
                }
                catch(Exception ex)
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

            public bool UpdateExam(Exam exam)
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();
                    List<string> exludes = new List<string>();
                    Type examType = exam.GetType();
                    PropertyInfo[] pros = examType.GetProperties().Where(p => p.GetValue(exam, null) == null).ToArray();
                    foreach (PropertyInfo pro in pros)
                    {
                        exludes.Add(pro.Name);
                    }


                    return this.repositoryUOW.ExamRepository.Update(exam, exludes);
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

            public bool UpdateSolution(Solution solution)
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();
                    List<string> exludes = new List<string>();
                    Type solutionType = solution.GetType();
                    PropertyInfo[] pros = solutionType.GetProperties().Where(p => p.GetValue(solution, null) == null).ToArray();
                    foreach (PropertyInfo pro in pros)
                    {
                        exludes.Add(pro.Name);
                    }


                    return this.repositoryUOW.SolutionRepository.Update(solution, exludes);
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

            public bool UpdateEvent(Event event_var)
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();
                    List<string> exludes = new List<string>();
                    Type eventType = event_var.GetType();
                    PropertyInfo[] pros = eventType.GetProperties().Where(p => p.GetValue(event_var, null) == null).ToArray();
                    foreach (PropertyInfo pro in pros)
                    {
                        exludes.Add(pro.Name);
                    }


                    return this.repositoryUOW.EventRepository.Update(event_var, exludes);
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
        public bool UpdateOrder(Order order)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                return this.repositoryUOW.OrderRepository.Update(order);
            }
            catch(Exception ex)
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
        public List<Order> GetAllOrders()
        {
            try
            {


                this.repositoryUOW.HelperOledb.OpenConnection();

                List<Order> orders = new List<Order>();

                
                orders = this.repositoryUOW.OrderRepository.GetAll();

                return orders;




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
        public List<Registered> GetAllRegistereds()
        {
            try
            {


                this.repositoryUOW.HelperOledb.OpenConnection();

                List<Registered> orders = new List<Registered>();


                orders = this.repositoryUOW.RegisteredRepository.GetAll();

                return orders;




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



        [HttpPost]
        [RequestSizeLimit(104857600)]
        public Book UpdateFullBook([FromForm] string model)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                

                JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
                jsonSerializerOptions.PropertyNameCaseInsensitive = true;
                Book modelBook = JsonSerializer.Deserialize<Book>(model, jsonSerializerOptions);

                if (HttpContext.Request.Form.Files.Count != 0)
                {
                    foreach (IFormFile file in HttpContext.Request.Form.Files)
                    {
                        string ext = Path.GetExtension(file.FileName);
                        //Console.WriteLine($"FileName = '{file.FileName}', ContentType = '{file.ContentType}'");

                        if (string.IsNullOrEmpty(ext))
                        {
                            ext = (file.ContentType ?? "").ToLowerInvariant() switch
                            {
                                "image/jpeg" => ".jpg",
                                "image/png" => ".png",
                                "image/gif" => ".gif",
                                "application/pdf" => ".pdf",
                                _ => throw new Exception("Unsupported file type")
                            };
                        }

                        if(ext == ".pdf")
                        {
                            modelBook.Pdf_url_book = this.repositoryUOW.BookRepository.ChangeFile(file, modelBook.BookID);

                        }
                        else
                        {
                            modelBook.BookImagePath = this.repositoryUOW.BookRepository.ChangeImage(file, modelBook.BookID);

                        }
                    }

                }


                if (this.repositoryUOW.BookRepository.Update(modelBook))
                    return modelBook;
                return null;

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
        [RequestSizeLimit(104857600)]
        public bool CreateNewBook([FromForm] string model)
        {
            try
            {
                bool hasImage = false;
                this.repositoryUOW.HelperOledb.OpenConnection();
                    this.repositoryUOW.HelperOledb.OpenTransaction();
                JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
                jsonSerializerOptions.PropertyNameCaseInsensitive = true;
                Book modelBook = JsonSerializer.Deserialize<Book>(model, jsonSerializerOptions);
                bool suceed = this.repositoryUOW.BookRepository.Create(modelBook);
                modelBook.BookID = this.repositoryUOW.BookRepository.GetLastID();
                if (HttpContext.Request.Form.Files.Count != 0)
                {
                    foreach(IFormFile file in HttpContext.Request.Form.Files) {
                        string ext = Path.GetExtension(file.FileName);
                        //Console.WriteLine($"FileName = '{file.FileName}', ContentType = '{file.ContentType}'");

                        if (string.IsNullOrEmpty(ext))
                        {
                            ext = (file.ContentType ?? "").ToLowerInvariant() switch
                            {
                                "image/jpeg" => ".jpg",
                                "image/png" => ".png",
                                "image/gif" => ".gif",
                                "application/pdf" => ".pdf",
                                _ => throw new Exception("Unsupported file type")
                            };
                        }

                        if(ext == ".pdf")
                        {
                            modelBook.Pdf_url_book = this.repositoryUOW.BookRepository.ChangeFile(file, modelBook.BookID);
                        }
                        else
                        {
                            //hasImage = true;
                            //IFormFile file = HttpContext.Request.Form.Files[0];
                            modelBook.BookImagePath = this.repositoryUOW.BookRepository.ChangeImage(file, modelBook.BookID);
                        }
                        this.repositoryUOW.BookRepository.Update(modelBook);


                    }

                }

                this.repositoryUOW.HelperOledb.Commit();
                return suceed;
            

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

        [HttpGet]

        public List<RegisteredComments> GetRegisteredReviews(string registeredID)
        {
            try {
                this.repositoryUOW.HelperOledb.OpenConnection();
                var comments = this.repositoryUOW.ReviewRepository.GetReviewsByRegistered(registeredID);
                return comments;
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

        [HttpPost]
        public bool SetAdmin(Registered reg)
        {
            try
            {
                reg.Role = "Admin";
                this.repositoryUOW.HelperOledb.OpenConnection();
                return this.repositoryUOW.RegisteredRepository.Update(reg);

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
        public bool RemoveAdmin(Registered reg)
        {
            try
            {
                reg.Role = "User";
                this.repositoryUOW.HelperOledb.OpenConnection();
                return this.repositoryUOW.RegisteredRepository.Update(reg);

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
