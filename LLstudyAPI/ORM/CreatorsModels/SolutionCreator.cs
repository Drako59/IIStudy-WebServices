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
                SolutionID = Convert.ToString(dataReader["Solution_ID"]),
                ExamID = Convert.ToString(dataReader["Exam_ID"]),
                Access = Convert.ToBoolean(dataReader["Access"]),
                File_path_url = Convert.ToString("File_path_url"),
                Solution_Name = Convert.ToString("Solution_Name"),
                Solution_Year = Convert.ToString("Solution_year")
            };
        }
    }
}
