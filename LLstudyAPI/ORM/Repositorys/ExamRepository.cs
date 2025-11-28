using LLStudy_Models.Models;
using LLstudyWS.ORM.CreatorsModels;
using System.Data;
using static System.Reflection.Metadata.BlobBuilder;

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

    }
}
