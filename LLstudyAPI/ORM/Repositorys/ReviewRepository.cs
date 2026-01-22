using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using LLstudyWS.ORM.CreatorsModels;
using System.Data;

namespace LLstudyWS.ORM
{
    public class ReviewRepository : Repository<Review>, IRepository<Review>
    {
        public ReviewRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }

        public List<ViewReview> GetReviewsByBook(string bookID)
        {
            List<ViewReview> reviews = new List<ViewReview>();
            //string sql = "SELECT * FROM Reviews WHERE BookID = @BookID";
            string sql = $@"SELECT   Reviews.RegisteredID AS [RegisteredID],* FROM  Reviews
                                    INNER JOIN ( Registereds
                                       
                                    ) ON Registereds.RegisteredID = Reviews.RegisteredID
                                WHERE
                                    BookID = @BookID";
            this.helperOledb.AddParameter("@BookID",bookID);

            using(IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {
                    reviews.Add(this.moderlRefCreator.CreateModel<ViewReview>(reader));
                }
            }
            return reviews;
        }
    }
}
