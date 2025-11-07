using LLStudy_Models.Models;
using System.Data;

namespace LLstudyWS.ORM
{
    public class ExamRepository : Repository, IRepository<Exam>
    {
        public bool Create(Exam model)
        {
            string sql = @"INSERT INTO
                        Exams (
                            Exam_Name,
                            Exam_year,
                            categoryID,
                            acsses,
                            file_path_url
                        )
                    VALUES
                        (@NAME, @YEAR, @CATE_VAR, @ACSSES, @PATH)";

            this.helperOledb.AddParameter("@NAME", model.Exam_name);
            this.helperOledb.AddParameter("@CATE_VAR", model.CatregoryID);
            this.helperOledb.AddParameter("@YEAR", model.Exam_year);
            this.helperOledb.AddParameter("@NAME", model.Access.ToString());
            this.helperOledb.AddParameter("@PATH", model.File_path_url);

            return this.helperOledb.Insert(sql) > 0;
        }

        public bool Delete(string id)
        {
            string sql = "DELETE * FROM Events WHERE Exam_Name = @NAME";
            this.helperOledb.AddParameter("@NAME", id);
            return this.helperOledb.Delete(sql) > 0;
        }

        public List<Exam> GetAll()
        {
            string sql = "SELECT * FROM Exams";
            List<Exam> exams = new List<Exam>();
            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {
                    exams.Add(this.modelCreators.ExamCreator.CreateModel(reader));
                }
            }
            return exams;
        }

        public Exam GetByID(string ID)
        {
            string sql = "SELECT * FROM Exams  WHERE Exam_ID = @ID";
            this.helperOledb.AddParameter("@ID", ID);
            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                reader.Read();
                return this.modelCreators.ExamCreator.CreateModel(reader);
            }
        }

        public bool Update(Exam model)
        {
            string sql = @"UPDATE Exams
                        SET
                            Exam_Name = @NAME,
                            Exam_year = @YEAR,
                            acsses = @ACS,
                            file_path_url = @PATH,
                            categoryID = @CA
                        WHERE
                            Exam_ID = @ID";
            this.helperOledb.AddParameter("@NAME", model.Exam_name);
            this.helperOledb.AddParameter("@PATH", model.File_path_url);
            this.helperOledb.AddParameter("@YEAR", model.Exam_year);
            this.helperOledb.AddParameter("@ACS", model.Access.ToString());
            this.helperOledb.AddParameter("@CA", model.CatregoryID);
            this.helperOledb.AddParameter("@ID", model.Exam_ID);
            return this.helperOledb.Update(sql) > 0;


        }
    }
}
