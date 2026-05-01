using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using LLstudyWS.ORM.CreatorsModels;
using System.Data;


namespace LLstudyWS.ORM
{
    public class SolutionRepository : Repository<Solution>, IRepository<Solution>
    {
        public SolutionRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }

        public List<Solution> GetByYear(string year)
        {
            string sql = $@"SELECT * FROM Solutions WHERE Solution_Year = @year";
            this.helperOledb.AddParameter("@year", year);
            List<Solution> solutions = new List<Solution>();
            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {
                    solutions.Add(this.moderlRefCreator.CreateModel<Solution>(reader));
                }
            }
            return solutions;
        }

        public List<Solution> GetBySubjectId(string subjectID)
        {
            string sql = $@"SELECT * FROM Solutions WHERE CategoryID = @subjectID";

            this.helperOledb.AddParameter("@subjectID", subjectID);
            List<Solution> solutions = new List<Solution>();
            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {
                    solutions.Add(this.moderlRefCreator.CreateModel<Solution>(reader));
                }
            }
            return solutions;

        }

        public List<Solution> GetSolutionsByExam(string examID)
        {
            string sql = $@"SELECT * FROM Solutions WHERE ExamID = @examID ORDER BY SolutionID";
            this.helperOledb.AddParameter("@examID", examID);

            using(IDataReader reader = this.helperOledb.Select(sql))
            {
                List<Solution> solutions = new List<Solution>();
                while (reader.Read())
                {
                    solutions.Add(this.moderlRefCreator.CreateModel<Solution>(reader));
                }
                return solutions;
            }


        }

        public string ChangeFile(IFormFile file, string solutionID)
        {


            if (file == null || file.Length == 0)
                throw new Exception("Empty file");

            //NEED TO ADD**********************************************************************
            //Registered reg2 = this.GetByID(registeredID);
            //File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "RegisteredImages",reg2.ImagePath));

            //string path = Path.Combine(Directory.GetCurrentDirectory()!, "App_Data","RegisteredsImages");
            string path = Path.Combine(Directory.GetCurrentDirectory()!, "wwwroot", "Files", "SolutionsFiles");

            Directory.CreateDirectory(path);

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

            string fileName = $"Solution{solutionID}{ext}";

            path = Path.Combine(path, fileName);
            //Console.WriteLine("********************************" + path);


            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }



            return fileName;
        }

        public bool CheckIfValidExam(string examID)
        {
            string sql = "SELECT COUNT(*) AS CountExams FROM Exams WHERE ExamID = @ExamID";
            this.helperOledb.AddParameter("@ExamID", examID);

            using(IDataReader  reader = this.helperOledb.Select(sql))
            {
                if (reader.Read())
                {
                    return Convert.ToInt16(reader["CountExams"]) > 0 ;
                }
                return false;
            }
        }
    }
}
