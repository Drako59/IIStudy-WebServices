using LLStudy_Models.Models;
using LLstudyWS.ORM.CreatorsModels;
using System.Data;
using static System.Reflection.Metadata.BlobBuilder;
using LLStudy_Models.ViewModels;

namespace LLstudyWS.ORM
{
    public class ExamRepository : Repository<Exam>, IRepository<Exam>
    {
        public ExamRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }
        public List<Exam> GetByYear(string year)
        {
            string sql = $@"SELECT * FROM Exams WHERE Exam_year = @year";
            this.helperOledb.AddParameter("@year", year);
            List<Exam> exams = new List<Exam>();
            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {
                    exams.Add(this.moderlRefCreator.CreateModel<Exam>(reader));
                }
            }
            return exams;
        }

        public List<Exam> GetBySubjectId(string subjectID)
        {
            string sql = $@"SELECT Books.BookID AS BookID,* FROM Exams WHERE CategoryID = @subjectID";
            
            this.helperOledb.AddParameter("@subjectID", subjectID);
            List<Exam> books = new List<Exam>();
            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {
                    books.Add(this.moderlRefCreator.CreateModel<Exam>(reader));
                }
            }
            return books;

        }

        public List<ExamDetails> GetExamsDetails()
        {
            string sql = $@"SELECT *, Subjects.SubjectID AS [SubjectID] FROM Exams INNER JOIN Subjects ON Exams.SubjectID = Subjects.SubjectID";

            List<ExamDetails> exams = new List<ExamDetails>();

            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {
                    exams.Add(this.moderlRefCreator.CreateModel<ExamDetails>(reader));
                }
                return exams;
            }
        }

        public string ChangeFile(IFormFile file, string examID)
        {


            if (file == null || file.Length == 0)
                throw new Exception("Empty file");

            //NEED TO ADD**********************************************************************
            //Registered reg2 = this.GetByID(registeredID);
            //File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "RegisteredImages",reg2.ImagePath));

            //string path = Path.Combine(Directory.GetCurrentDirectory()!, "App_Data","RegisteredsImages");
            string path = Path.Combine(Directory.GetCurrentDirectory()!, "wwwroot", "Files", "ExamsFiles");

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

            string fileName = $"Exam{examID}{ext}";

            path = Path.Combine(path, fileName);
            //Console.WriteLine("********************************" + path);


            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }



            return fileName;
        }

        public List<string> GetExamsYearsForSubject(string subjectID)
        {
            string sql = $@"SELECT DISTINCT LEFT(Exam_Year,4) AS ExamOnlyYear FROM Exams 
                                    WHERE SubjectID = @SubjectID 
                                    ORDER BY LEFT(Exam_Year, 4) DESC";
            this.helperOledb.AddParameter("@SubjectID",subjectID);

            List<string> years = new List<string>();
            using(IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {
                    years.Add(Convert.ToString(reader["ExamOnlyYear"]));
                }
                return years;
            }
        }
    }
}
