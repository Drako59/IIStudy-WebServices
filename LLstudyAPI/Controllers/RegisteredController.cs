using LLStudy_Models.Models;
using LLstudyWS.ORM.Repositorys;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost]
        public Registered Sign_In(string password, string? UserName = null, string email = null)
        {
            try
            {
                Registered register;
                this.repositoryUOW.HelperOledb.OpenConnection();
                if (email != null)
                {
                    register = this.repositoryUOW.RegisteredRepository.Login(password, email: email);
                }
                else
                {
                    register = this.repositoryUOW.RegisteredRepository.Login(password, username: UserName);
                }
                return register;

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
        public Registered Sign_Up(Registered reg)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                
                this.repositoryUOW.RegisteredRepository.Create(reg,new List<string>() { "Role"});

                reg = this.repositoryUOW.RegisteredRepository.Login(reg.Password,username: reg.UserName);

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
        public List<Book> GetUserBooks(string userName)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                List<Book> books = new List<Book>();
                books = this.repositoryUOW.BookRepository.GetUserNameBooks(userName);
                foreach(Book book in books)
                    Console.WriteLine($@"BookName: {book.Book_name}");
                Console.WriteLine("Test");
                return books;
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
