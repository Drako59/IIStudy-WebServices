using LLStudy_Models.Models;
using System.Data;

namespace LLstudyWS.ORM
{
    public class CategoryCreator: IModelCreator<Subject>
    {

        public Subject CreateModel(IDataReader dataReader) { return new Subject() 
        {
            SubjectID = Convert.ToString(dataReader["SubjectID"]),
            Subject_name = Convert.ToString(dataReader["Subject_name"])
        }; }
    }
}
