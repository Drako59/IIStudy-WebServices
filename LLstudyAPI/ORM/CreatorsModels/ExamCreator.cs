using LLStudy_Models.Models;
using System.Data;

namespace LLstudyWS.ORM
{
    public class ExamCreator: IModelCreator<Exam>
    {
        public Exam CreateModel(IDataReader dataReader)
        {
            return new Exam()
            {
                ExamID = Convert.ToString(dataReader["ExamID"]),
                SubjectID = Convert.ToString(dataReader["CategoryID"]),
                Access = Convert.ToBoolean(dataReader["Access"]),
                File_path_url = Convert.ToString("File_path_url"),
                Exam_Name = Convert.ToString("Exam_Name"),
                Exam_Year = Convert.ToString("Exam_Year")
            };
        }
    }
}
