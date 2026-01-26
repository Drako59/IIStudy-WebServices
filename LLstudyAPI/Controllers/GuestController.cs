using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LLStudy_Models;
using LLStudy_Models.ViewModels;
using System.Security.Cryptography.X509Certificates;
using LLstudyWS.ORM.Repositorys;
using LLStudy_Models.Models;
using System.Security.Permissions;
using System.Data;
using LLStudy_Models.ViewModels;

using System.Text.Json;
using System.Reflection.Metadata.Ecma335;
namespace LLstudyWS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GuestController : ControllerBase
    {


        private void debugList<T>(List<T> list)
        {
            foreach (T var in list)
            {
                Console.WriteLine(var.ToString());
            }
        }
        RepositoryUOW repositoryUOW;
        public GuestController()
        {
            this.repositoryUOW = new RepositoryUOW();
        }
        [HttpPost]
        public Registered SignIn(SignInViewModel SignInModel)
        {
            try
            {
                Registered reg = new Registered() {
                    UserName="None",
                    Email = "None",
                    Password = "None",
                    RegisteredSalt = "None",
                    Role = "User",
                    Birth = "None",
                    ImagePath = "None"
               

                };
                this.repositoryUOW.HelperOledb.OpenConnection();
                reg.RegisteredID =  this.repositoryUOW.RegisteredRepository.LoginID(SignInModel.Password, SignKey: SignInModel.SignKey);
                if (reg.RegisteredID == null)
                    return null;
                return reg;
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
        public Registered SignInDetails(SignInViewModel model)
        {
            try
            {
                //Console.WriteLine(@$"Email: {SignInModel.Email}, UserName: {SignInModel.UserName}");

                Registered register;
                this.repositoryUOW.HelperOledb.OpenConnection();
                return this.repositoryUOW.RegisteredRepository.Login(model.Password, signTool: model.SignKey);
                

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
        public Registered SignUp([FromBody]Registered model) //Registered
        {
            //string json = HttpContext.Request.Form["model"];
            //Registered user = JsonSerializer
            //List<IFormFile> file = null;
            //if (HttpContext.Request.Form.Files.Count > 0)
            //{
            //    for (int i = 0; i < HttpContext.Request.Form.Files.Count; i++)
            //    {
            //        file.Add(HttpContext.Request.Form.Files[i]);
            //    }

            //}

           
            try
            {
                //return null;
                this.repositoryUOW.HelperOledb.OpenConnection();
                string regID;
                this.repositoryUOW.RegisteredRepository.CreateWithHash(model);
                regID = this.repositoryUOW.RegisteredRepository.LoginID(model.Password, SignKey: model.UserName);
                //string path = @$"{Directory.GetCurrentDirectory()}\wwwroot\Images\[DIRNAME]\{reg.RegisteredID}";
                model.RegisteredID = regID;

                if (model.RegisteredID == null)
                    return null;

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
        public List<Book> GetBooks(string? subjectID = null, string? author_name = null,string? search = null, string? book_name = null, string? price_min = null, string? price_max = null,string? type = null) {

            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();


                if (search == null && subjectID == null && author_name == null && book_name == null && price_min == null && price_max == null && type == null)
                    return this.repositoryUOW.BookRepository.GetAll();

                List<Book> books = new List<Book>();
                if (search != null)
                    books.AddRange(this.repositoryUOW.BookRepository.GetByName(search));

                
                return books;
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
        [HttpGet]
        public ViewExamsModel GetExams(string year = null, string subjectID = null, int pages = 0)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                List<Exam> temp = new List<Exam>();

                List<Solution> temp_solutions = new List<Solution>();
                ViewExamsModel viewExamsModel = new ViewExamsModel();

                if (year == null && subjectID == null && pages == 0)
                {
                    Console.WriteLine("here");

                    viewExamsModel.Exams = this.repositoryUOW.ExamRepository.GetAll();
                    viewExamsModel.Solutions = this.repositoryUOW.SolutionRepository.GetAll();
                    Console.WriteLine("here");
                    this.debugList(viewExamsModel.Exams);
                    this.debugList(viewExamsModel.Solutions);
                    Console.WriteLine("Here");
                    viewExamsModel.Exams = this.repositoryUOW.ExamRepository.GetAll();
                    viewExamsModel.Solutions = this.repositoryUOW.SolutionRepository.GetAll();
                    Console.WriteLine("here");
                    this.debugList(viewExamsModel.Exams);
                    this.debugList(viewExamsModel.Solutions);

                    return viewExamsModel;

                }


                if (year != null)
                {
                    temp.AddRange(this.repositoryUOW.ExamRepository.GetByYear(year));
                    temp_solutions.AddRange(this.repositoryUOW.SolutionRepository.GetByYear(year));

                }

                if (subjectID != null)
                {
                    temp.AddRange(this.repositoryUOW.ExamRepository.GetBySubjectId(subjectID));
                    temp_solutions.AddRange(this.repositoryUOW.SolutionRepository.GetBySubjectId(subjectID));

                }

                viewExamsModel.Exams = temp;
                viewExamsModel.Solutions = temp_solutions;
                return viewExamsModel;


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
        public List<Event> Calender() //string year, string month
        {

            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                List<Event> events = this.repositoryUOW.EventRepository.GetAll() ;
                return events;


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
        public List<Order> GetAllOrders()
        {
            try
            {
                

                this.repositoryUOW.HelperOledb.OpenConnection();

                List<Order> orders = new List<Order>();

                if (false)
                {
                    return null;
                }
                else
                {
                    orders = this.repositoryUOW.OrderRepository.GetAll();
                }

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


        [HttpGet]

        public ViewBookViewModel GetBookFullView(string bookID)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();

                ViewBookViewModel viewBookViewModel = new ViewBookViewModel();
                viewBookViewModel.reviews = new List<ViewReview>();
                viewBookViewModel.book = this.repositoryUOW.BookRepository.GetByID(bookID);
                viewBookViewModel.reviews = this.repositoryUOW.ReviewRepository.GetReviewsByBook(bookID);
                viewBookViewModel.Rate = this.repositoryUOW.BookRepository.GetBookRate(bookID);
                return viewBookViewModel;

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

        public Book GetBook(string bookID)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();


                return this.repositoryUOW.BookRepository.GetByID(bookID);

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