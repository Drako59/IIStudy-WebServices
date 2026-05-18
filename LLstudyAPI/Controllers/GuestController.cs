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
using LLStudy_Models.ViewModels.Guest;
namespace LLstudyWS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

   
    public class GuestController : ControllerBase
    {
        readonly string BooksPath = Path.Combine(Directory.GetCurrentDirectory()!, "wwwroot", "Images", "BooksImages");

        Dictionary<string, string> subjectsDict;

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
        public Registered SignIn(SignInViewModel signInModel)
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
                reg.RegisteredID =  this.repositoryUOW.RegisteredRepository.LoginID(signInModel.Password, SignKey: signInModel.SignKey);
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
        public Registered AdminSignIn(SignInViewModel signInModel )
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                Registered reg = new Registered()
                {
                    UserName = "None",
                    Email = "None",
                    Password = "None",
                    RegisteredSalt = "None",
                    Role = "User",
                    Birth = "None",
                    ImagePath = "None"


                };
                reg.RegisteredID = this.repositoryUOW.RegisteredRepository.AdminLoginID(signInModel.Password, SignKey: signInModel.SignKey);
                if (reg.RegisteredID == null)
                    return null;
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
                    return this.repositoryUOW.BookRepository.GetExistBooks();
                    //return this.repositoryUOW.BookRepository.GetAll();


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
        public ViewExamsModel GetExamsWithSolutions(string year = null, string subjectID = null, int pages = 0) //Not In Use
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                List<Exam> temp = new List<Exam>();

                List<Solution> temp_solutions = new List<Solution>();
                ViewExamsModel viewExamsModel = new ViewExamsModel();

                if (year == null && subjectID == null && pages == 0)
                {
                    

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
        public List<ExamDetails> GetExams()
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                //return new List<ExamDetails>();
                return this.repositoryUOW.ExamRepository.GetExamsDetails();
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
        public List<Solution> GetSolutions(string examID)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                return this.repositoryUOW.SolutionRepository.GetSolutionsByExam(examID);
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

        public ViewBookViewModel GetBookFullView(string bookID)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();

                //ViewBookViewModel viewBookViewModel = new ViewBookViewModel();
                //viewBookViewModel.reviews = new List<ViewReview>();
                //viewBookViewModel.book = this.repositoryUOW.BookRepository.GetByID(bookID);
                //viewBookViewModel.reviews = this.repositoryUOW.ReviewRepository.GetReviewsByBook(bookID);
                //viewBookViewModel.reviewsNumber = viewBookViewModel.reviews.Count();
                //viewBookViewModel.Rate = this.repositoryUOW.BookRepository.GetBookRate(bookID);

                ViewBookViewModel viewBookViewModel = this.repositoryUOW.BookRepository.GetFullBook(bookID);

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
        public List<ViewReview> GetBookReviews(string bookID)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                return this.repositoryUOW.BookRepository.GetReviewsByBook(bookID);
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

        public List<ViewBookViewModel> GetAllBookFullView() //with reviews, not in use .
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();


                return this.repositoryUOW.BookRepository.GetFullBooks();

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

        public List<BookShownDesktop> GetDesktopBooks()
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();


                return this.repositoryUOW.BookRepository.GetDesktopBooks() ;

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

        [HttpGet]
        public IActionResult GetBookImage(string bookID)
        {
            try
            {
                string AbsoultePath;
                this.repositoryUOW.HelperOledb.OpenConnection();
                Book book = this.repositoryUOW.BookRepository.GetByID(bookID);
                if (book.BookImagePath != null && book.BookImagePath.ToLower() != "none" && System.IO.File.Exists(Path.Combine(this.BooksPath,book.BookImagePath)))
                {
                    AbsoultePath = Path.Combine(this.BooksPath, book.BookImagePath);

                }
                else
                {
                    AbsoultePath = Path.Combine(this.BooksPath, "PlaceHolder.jpg");
                }

                var (stream, contentType) = this.repositoryUOW.RegisteredRepository.GetImage(AbsoultePath);

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
        public List<Subject> GetAllSubjects() //Not In Use I think
        {
            try 
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                return this.repositoryUOW.SubjectRepository.GetAll();
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
        public Dictionary<string,string> GetAllSubjectsDict()
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                return this.repositoryUOW.SubjectRepository.GetSubjectsDict();
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
        public List<string> GetSubjectsNamesList() //Not In Use I think
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                return this.repositoryUOW.SubjectRepository.GetSubjectsNamesList();
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
        public List<SubjectDetails> GetBooksSubjectsDetails()
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                return this.repositoryUOW.SubjectRepository.GetBooksSubjectsDetailsList();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            finally{
                this.repositoryUOW.HelperOledb.CloseConnection();
            }
        }

        [HttpGet]
        public List<SubjectDetails> GetExamsSubjectsDetails()
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                return this.repositoryUOW.SubjectRepository.GetExamsSubjectsDetailsList();
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
        public List<Subject> GetSubjects()
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                return this.repositoryUOW.SubjectRepository.GetAll();
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
        public List<EventDetail> GetEventsDetails()
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                return this.repositoryUOW.EventRepository.GetEventsDetails();
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
        public List<string> ExamsYearsListBySubject(string subjectID)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                return this.repositoryUOW.ExamRepository.GetExamsYearsForSubject(subjectID);
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

    }
}