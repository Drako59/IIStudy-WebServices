using LLStudy_Models.Models;
using System.Data;

namespace LLstudyWS.ORM
{
    public class ReviewCreator : IModelCreator<Review>
    {
        public Review CreateModel(IDataReader dataReader)
        {
            return new Review() 
            {
                ReviewID = Convert.ToString(dataReader["ReviewID"]),
                Comment = Convert.ToString(dataReader["Comment"]),
                BookID = Convert.ToString(dataReader["Book_ID"]),
            };
        }
    }
}
