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
                Review_ID = Convert.ToString(dataReader["Review_ID"]),
                Rate = Convert.ToString(dataReader["Rate"]),
                Comment = Convert.ToString(dataReader["Comment"]),
                Book_ID = Convert.ToString(dataReader["Book_ID"]),
                UserName = Convert.ToString(dataReader["UserName"])
            };
        }
    }
}
