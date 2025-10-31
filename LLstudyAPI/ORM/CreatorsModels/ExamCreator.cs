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
                Exam_ID = Convert.ToString(dataReader["Exam_ID"]),
                CatregoryID = Convert.ToString(dataReader["categoryID"]),
                Access = Convert.ToBoolean(dataReader["acsses"]),
                File_path_url = Convert.ToString("file_path_url"),
                Exam_name = Convert.ToString("Exam_Namne"),
                Exam_year = Convert.ToString("Exam_year")
            };
        }
    }
}
