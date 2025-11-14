using LLStudy_Models.Models;
using System.Data;

namespace LLstudyWS.ORM
{
    public class CategoryCreator: IModelCreator<Category>
    {

        public Category CreateModel(IDataReader dataReader) { return new Category() 
        {
            SubjectID = Convert.ToString(dataReader["SubjectID"]),
            Subject_name = Convert.ToString(dataReader["Subject_name"])
        }; }
    }
}
