using LLStudy_Models.Models;
using System.Data;

namespace LLstudyWS.ORM
{
    public class CreateReview : IModelCreator<Review>
    {
        public Review CreateModel(IDataReader dataReader)
        {
            return new Review() 
            {
                Review_ID = Convert.ToString(dataReader["review_ID"]),
                Rate = Convert.ToString(dataReader["rate"]),
                Comment = Convert.ToString(dataReader["comment"]),
                Book_ID = Convert.ToString(dataReader["book_ID"]),
                UserName = Convert.ToString(dataReader["UserName"])
            };
        }
    }
}
