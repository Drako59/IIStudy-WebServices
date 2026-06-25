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
            readonly string SolutionsPdfPath = Path.Combine(Directory.GetCurrentDirectory()!, "wwwroot", "Files", "SolutionsFiles");
            RepositoryUOW repositoryUOW;
            public AdminController() {
                this.repositoryUOW = new RepositoryUOW();
            }


            [HttpPost]
            public bool CreateEvent(Event event_var)
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

            public bool DeleteEvent([FromBody] Event Event)
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
            public bool RemoveExam(Exam exam)
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();
                    exam.IsDeleted = true;
                    return this.repositoryUOW.ExamRepository.Update(exam, exludes: new List<string>() { nameof(exam.File_path_url), nameof(exam.Exam_Year), nameof(exam.Exam_Name) });
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

            public bool RestoreExam(Exam exam)
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();
                    exam.IsDeleted = false;
                    return this.repositoryUOW.ExamRepository.Update(exam,exludes: new List<string>() { nameof(exam.File_path_url),nameof(exam.Exam_Year),nameof(exam.Exam_Name)});
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

            public bool DeleteSolution(Solution solution) 
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();
                    this.repositoryUOW.HelperOledb.OpenTransaction();
                    Solution solution_db = this.repositoryUOW.SolutionRepository.GetByID(solution.SolutionID);

                    if (solution_db.File_path_url != null && solution_db.File_path_url.ToLower() != "none") {
                        if (!solution_db.File_path_url.StartsWith($"Solution{solution_db.SolutionID}"))
                            throw new Exception(message: "file path value isn't valid.");



                        string file_path_to_delete = Path.Combine(this.SolutionsPdfPath, solution_db.File_path_url);
                        this.repositoryUOW.SolutionRepository.DeleteFile(file_path_to_delete);
                    }
                    bool status =  this.repositoryUOW.SolutionRepository.Delete(solution.SolutionID);
                    this.repositoryUOW.HelperOledb.Commit();
                    return status;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    this.repositoryUOW.HelperOledb.RollBack();
                    return false; ;
                }
                finally
                {
                    this.repositoryUOW.HelperOledb.CloseConnection();
                }
            }

            [HttpPost]

            public bool AddExam(Exam exam) //To Remove
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
            public bool AddSolution(Exam exam) //To Remove
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
            public bool AddBook(Book book) //To Remove
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

            public bool UpdateBook(Book book) //To remove
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

            //[HttpPost] 

            //public bool UpdateExam(Exam exam) //To remove
            //{
            //    try
            //    {
            //        this.repositoryUOW.HelperOledb.OpenConnection();
            //        List<string> exludes = new List<string>();
            //        Type examType = exam.GetType();
            //        PropertyInfo[] pros = examType.GetProperties().Where(p => p.GetValue(exam, null) == null).ToArray();
            //        foreach (PropertyInfo pro in pros)
            //        {
            //            exludes.Add(pro.Name);
            //        }


            //        return this.repositoryUOW.ExamRepository.Update(exam, exludes);
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
            //[HttpPost]

            //public bool UpdateSolution(Solution solution) //NOT IN USE!!!
            //{
            //    try
            //    {
            //        this.repositoryUOW.HelperOledb.OpenConnection();
            //        List<string> exludes = new List<string>();
            //        Type solutionType = solution.GetType();
            //        PropertyInfo[] pros = solutionType.GetProperties().Where(p => p.GetValue(solution, null) == null).ToArray();
            //        foreach (PropertyInfo pro in pros)
            //        {
            //            exludes.Add(pro.Name);
            //        }


            //        return this.repositoryUOW.SolutionRepository.Update(solution, exludes);
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

                    List<Registered> regs = new List<Registered>();


                    regs = this.repositoryUOW.RegisteredRepository.GetAll(exludes: new List<string>() { nameof(Registered.Password), nameof(Registered.RegisteredSalt)});
                    regs.ForEach(r => r.Password = "NoneNoneNone");
                    regs.ForEach(r => r.RegisteredSalt = "None");
                    return regs;




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

            public Solution UpdateSolution([FromForm] string model)
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();

                
                    JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
                    jsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    Solution modelSolution = JsonSerializer.Deserialize<Solution>(model, jsonSerializerOptions);

                    if (!this.repositoryUOW.SolutionRepository.CheckIfValidExam(modelSolution.ExamID))
                        return null;

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

                                    "application/pdf" => ".pdf",
                                    _ => throw new Exception("Unsupported file type")
                                };
                            }


                            modelSolution.File_path_url = this.repositoryUOW.SolutionRepository.ChangeFile(file, modelSolution.SolutionID);



                        }

                    }


                    if (this.repositoryUOW.SolutionRepository.Update(modelSolution))
                        return modelSolution;
                    return null;

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

            public Exam UpdateExam([FromForm] string model)
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();


                    JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
                    jsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    Exam modelExam = JsonSerializer.Deserialize<Exam>(model, jsonSerializerOptions);

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
                                
                                    "application/pdf" => ".pdf",
                                    _ => throw new Exception("Unsupported file type")
                                };
                            }


                            modelExam.File_path_url = this.repositoryUOW.ExamRepository.ChangeFile(file, modelExam.ExamID);

                        
                        
                        }

                    }


                    if (this.repositoryUOW.ExamRepository.Update(modelExam))
                        return modelExam;
                    return null;

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

            public bool CreateNewSolution([FromForm] string model)
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();
                    this.repositoryUOW.HelperOledb.OpenTransaction();
                
                    JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
                    jsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    Solution modelSolution = JsonSerializer.Deserialize<Solution>(model, jsonSerializerOptions);
                    if (!this.repositoryUOW.SolutionRepository.CheckIfValidExam(modelSolution.ExamID))
                    {
                        return false;
                    }
                    bool succeed = this.repositoryUOW.SolutionRepository.Create(modelSolution);

                    modelSolution.SolutionID = this.repositoryUOW.SolutionRepository.GetLastID();
                
                    if (HttpContext.Request.Form.Files.Count > 0)
                    {
                        IFormFile file = HttpContext.Request.Form.Files[0];
                        string ext = Path.GetExtension(file.FileName);
                        if (string.IsNullOrEmpty(ext))
                        {
                            ext = (file.ContentType ?? "").ToLowerInvariant() switch
                            {
                                "application/pdf" => ".pdf",
                                _ => throw new Exception("Unsupported file type")
                            };
                        }

                        modelSolution.File_path_url = this.repositoryUOW.SolutionRepository.ChangeFile(file, modelSolution.SolutionID);
                        this.repositoryUOW.SolutionRepository.Update(modelSolution);
                    }
                    this.repositoryUOW.HelperOledb.Commit();

                    return succeed;

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
            [RequestSizeLimit(104857600)]

            public bool CreateNewExam([FromForm] string model)
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();
                    this.repositoryUOW.HelperOledb.OpenTransaction();
                    JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
                    jsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    Exam modelExam = JsonSerializer.Deserialize<Exam>(model, jsonSerializerOptions);

                    bool succeed = this.repositoryUOW.ExamRepository.Create(modelExam);

                    modelExam.ExamID = this.repositoryUOW.ExamRepository.GetLastID();
                    if(HttpContext.Request.Form.Files.Count > 0)
                    {
                        IFormFile file = HttpContext.Request.Form.Files[0];
                        string ext = Path.GetExtension(file.FileName);
                        if (string.IsNullOrEmpty(ext))
                        {
                            ext = (file.ContentType ?? "").ToLowerInvariant() switch
                            {
                                "application/pdf" => ".pdf",
                                _ => throw new Exception("Unsupported file type")
                            };
                        }

                        modelExam.File_path_url = this.repositoryUOW.ExamRepository.ChangeFile(file, modelExam.ExamID);
                        this.repositoryUOW.ExamRepository.Update(modelExam);
                    }
                    this.repositoryUOW.HelperOledb.Commit();

                    return succeed;

                }
                catch(Exception ex)
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


                        }
                        this.repositoryUOW.BookRepository.Update(modelBook);

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
            public bool SetAdmin([FromBody] Registered reg)
            {
                try
                {
                    reg.Role = "Admin";
                    this.repositoryUOW.HelperOledb.OpenConnection();
                    return this.repositoryUOW.RegisteredRepository.Update(reg, exludes: new List<string>() { nameof(Registered.Password), nameof(Registered.RegisteredSalt),nameof(Registered.Phone),nameof(Registered.IsBanned),nameof(Registered.Email),nameof(Registered.UserName)});

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
            public bool RemoveAdmin([FromBody] Registered reg)
            {
                try
                {
                    reg.Role = "User";
                    this.repositoryUOW.HelperOledb.OpenConnection();
                    return this.repositoryUOW.RegisteredRepository.Update(reg, exludes: new List<string>() { nameof(Registered.Password), nameof(Registered.RegisteredSalt), nameof(Registered.Phone), nameof(Registered.IsBanned), nameof(Registered.Email), nameof(Registered.UserName) });

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
            public List<OrderBook> GetOrderBooks(string orderID)
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();
                    return this.repositoryUOW.OrderRepository.GetOrderBooks(orderID);
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
            public bool CreateSubject([FromBody] Subject subject)
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();
                    return this.repositoryUOW.SubjectRepository.Create(subject);
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
            public bool EditSubject([FromBody] Subject subject)
            {
                try
                {
                    this.repositoryUOW.HelperOledb.OpenConnection();
                    return this.repositoryUOW.SubjectRepository.Update(subject);
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


    }
}
