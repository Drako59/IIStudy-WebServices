using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LLStudy_Models;
using LLStudy_Models.ViewModels;
using System.Security.Cryptography.X509Certificates;
using LLstudyWS.ORM.Repositorys;
using LLStudy_Models.Models;
using System.Security.Permissions;
using System.Data;
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

                viewBookViewModel.book = this.repositoryUOW.BookRepository.GetByID(bookID);
                viewBookViewModel.reviews = this.repositoryUOW.ReviewRepository.GetReviewsByBook(bookID);
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