using LLStudy_Models.Models;
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
        public bool RemoveBook(Book book)
        {
            try
            {
                this.repositoryUOW.HelperOledb.CloseConnection();
                return this.repositoryUOW.BookRepository.Delete(book.BookID);

            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                this.repositoryUOW.HelperOledb.CloseConnection();
            }
        }

        [HttpDelete]
        public bool RemoveReview(string reviewID)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();

                return this.repositoryUOW.ReviewRepository.Delete(reviewID);

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
        [HttpDelete]

        public bool RemoveEvent(string eventID)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();

                return this.repositoryUOW.EventRepository.Delete(eventID);

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

        [HttpDelete]
        public bool RemoveExam(string examID)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                return this.repositoryUOW.ExamRepository.Delete(examID);
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

        [HttpDelete]

        public bool RemoveSolution(string solutionID)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                return this.repositoryUOW.SolutionRepository.Delete(solutionID);
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

        public Book UpdateFullBook([FromForm] string model)
        {
            try
            {
                bool hasImage = false;
                this.repositoryUOW.HelperOledb.OpenConnection();
                

                JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
                jsonSerializerOptions.PropertyNameCaseInsensitive = true;
                Book modelBook = JsonSerializer.Deserialize<Book>(model, jsonSerializerOptions);

                if (HttpContext.Request.Form.Files.Count != 0)
                {
                    hasImage = true;
                    IFormFile file = HttpContext.Request.Form.Files[0];
                    modelBook.BookImagePath = this.repositoryUOW.BookRepository.ChangeImage(file, modelBook.BookID);


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
                hasImage = true;
                IFormFile file = HttpContext.Request.Form.Files[0];
                modelBook.BookImagePath = this.repositoryUOW.BookRepository.ChangeImage(file, modelBook.BookID);
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
}
    }
