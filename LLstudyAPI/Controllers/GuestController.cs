using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LLStudy_Models;
using LLStudy_Models.ViewModels.Guest;
using System.Security.Cryptography.X509Certificates;
using LLstudyWS.ORM.Repositorys;
using LLStudy_Models.Models;
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

                    viewExamsModel.Exams = this.repositoryUOW.ExamRpository.GetAll();
                    viewExamsModel.Solutions = this.repositoryUOW.SolutionRpository.GetAll();
                    Console.WriteLine("here");
                    this.debugList(viewExamsModel.Exams);
                    this.debugList(viewExamsModel.Solutions);

                    return viewExamsModel;

                }


                //if (year != null)
                //{
                //    temp.AddRange(this.repositoryUOW.ExamRpository.GetByYear(year));
                //    temp_solutions.AddRange(this.repositoryUOW.SolutionRpository.GetByYear(year));

                //}

                //if (subjectID != null)
                //{
                //    temp.AddRange(this.repositoryUOW.ExamRpository.GetBySubjectId(subjectID));
                //    temp_solutions.AddRange(this.repositoryUOW.SolutionRpository.GetBySubjectId(subjectID));

                //}
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
    }
}