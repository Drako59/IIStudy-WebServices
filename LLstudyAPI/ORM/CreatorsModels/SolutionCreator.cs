using LLStudy_Models.Models;
using System.Data;

namespace LLstudyWS.ORM
{
    public class SolutionCreator : IModelCreator<Solution>
    {
        public Solution CreateModel(IDataReader dataReader)
        {
            return new Solution()
            {
                Solution_ID = Convert.ToString(dataReader["Solution_ID"]),
                Exam_ID = Convert.ToString(dataReader["Exam_ID"]),
                CatregoryID = Convert.ToString(dataReader["categoryID"]),
                Access = Convert.ToBoolean(dataReader["acsses"]),
                File_path_url = Convert.ToString("file_path_url"),
                Solution_name = Convert.ToString("Solution_Namne"),
                Solution_year = Convert.ToString("Solution_year")
            };
        }
    }
}
