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
        [HttpPost]
        public Registered Sign_In(string password, string? UserName = null, string email = null)
        {
            try
            {
                Registered register;
                this.repositoryUOW.HelperOledb.OpenConnection();
                if (email != null)
                {
                    return this.repositoryUOW.RegisteredRepository.Login(password, email: email);
                }
                else if(UserName != null)
                {
                    return this.repositoryUOW.RegisteredRepository.Login(password, username: UserName);
                }
                else
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
        public string Sign_Up(Registered reg) //Registered
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                string regID;   
                this.repositoryUOW.RegisteredRepository.Create(reg,new List<string>() { "Role"});

                regID = this.repositoryUOW.RegisteredRepository.LoginID(reg.Password,username: reg.UserName);

                return regID;
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
        public ViewOwnedBooksModel GetUserBooks(string RegisteredID)
        {
            try
            {
                ViewOwnedBooksModel model = new ViewOwnedBooksModel();
                this.repositoryUOW.HelperOledb.OpenConnection();
                List<Book> books = new List<Book>();
                books = this.repositoryUOW.BookRepository.GetUserNameBooks(RegisteredID);
                model.Books = books;
                model.User = this.repositoryUOW.RegisteredRepository.GetByID(RegisteredID);
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

        [HttpPost]
        public string sign_in_ID(string password, string? userName = null, string? email = null)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                if (email != null)
                    return this.repositoryUOW.RegisteredRepository.LoginID(password, email: email);
                else if (userName != null)
                    return this.repositoryUOW.RegisteredRepository.LoginID(password, username: userName);
                else
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

        [HttpGet]
        public Registered profile(string ID)
        {
            try
            {
                Registered reg;
                this.repositoryUOW.HelperOledb.OpenConnection();
                reg = this.repositoryUOW.RegisteredRepository.GetByID(ID, new List<string>() {"HasErrors", "IsValid" } );
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
        public ViewShopingCartModel ViewShoppingCart(string ID)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();

                ViewShopingCartModel viewModel = new ViewShopingCartModel();

                viewModel.User = this.repositoryUOW.RegisteredRepository.GetByID(ID);
                viewModel.Books = this.repositoryUOW.BookRepository.GetShoppingCartBooks(ID);
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
        public ViewOrdersModel GetUserOrders(string ID)
        {
            try
            {
                
                this.repositoryUOW.HelperOledb.OpenConnection();
                ViewOrdersModel viewOrdersModel = new ViewOrdersModel();
                viewOrdersModel.Orders = this.repositoryUOW.OrderRepository.GetUserOrders(ID);
                viewOrdersModel.User = this.repositoryUOW.RegisteredRepository.GetByID(ID);
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

        public Order ViewOrderDetails(string ID)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
                return this.repositoryUOW.OrderRepository.GetByID(ID, new List<string>() { "IsValid", "HasErrors"});
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
        public Order Pay(PaymentViewModel payment)
        {
            try
            {
                this.repositoryUOW.HelperOledb.OpenConnection();
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
    }
}
