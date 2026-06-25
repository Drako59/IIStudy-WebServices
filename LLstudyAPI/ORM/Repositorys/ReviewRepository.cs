using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using LLstudyWS.ORM.CreatorsModels;
using System.Data;

namespace LLstudyWS.ORM
{
    public class ReviewRepository : Repository<Review>, IRepository<Review>
    {
        public ReviewRepository(DbHelperOledb helper, ModelCreatorReflection modelCretorRef) : base(helper,  modelCretorRef) { }

        //public List<ViewReview> GetReviewsByBook(string bookID)
        //{
        //    List<ViewReview> reviews = new List<ViewReview>();
        //    //string sql = "SELECT * FROM Reviews WHERE BookID = @BookID";
        //    string sql = $@"SELECT   Reviews.RegisteredID AS [RegisteredID],* FROM  Reviews
        //                            INNER JOIN ( Registereds
                                       
        //                            ) ON Registereds.RegisteredID = Reviews.RegisteredID
        //                        WHERE
        //                            BookID = @BookID";
        //    this.helperOledb.AddParameter("@BookID",bookID);

        //    using(IDataReader reader = this.helperOledb.Select(sql))
        //    {
        //        while (reader.Read())
        //        {
        //            reviews.Add(this.moderlRefCreator.CreateModel<ViewReview>(reader));
        //        }
        //    }
        //    return reviews;
        //}

        public List<RegisteredComments> GetReviewsByRegistered(string registeredID)
        {
            List<RegisteredComments> regReviews = new List<RegisteredComments>();
            string sql = @$"SELECT  Books.BookID  AS [BookID], * 
                                    ,(SELECT COUNT(*) FROM Likes WHERE ReviewID = Reviews.ReviewID AND IsDislike = True) AS [Dislikes]
                                    ,(SELECT COUNT(*) FROM Likes WHERE ReviewID = Reviews.ReviewID AND [IsLike] = True) AS [LikesCount]
                                    FROM Reviews
                                    INNER JOIN ( Books

                                    ) ON Books.BookID = Reviews.BookID 
                                WHERE 
                                    RegisteredID = @RegisteredID";
            this.helperOledb.AddParameter("@RegisteredID", registeredID);
            using(IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {
                    regReviews.Add(this.moderlRefCreator.CreateModel<RegisteredComments>(reader));
                }
            }
            return regReviews;
        }

        public string GetUserReviewOnBook(string registeredID, string bookID)
        {
            string sql = $@"SELECT ReviewID FROM Reviews WHERE RegisteredID = @RegisteredID AND BookID = @BookID";

            this.helperOledb.AddParameter("@RegisteredID", registeredID);
            this.helperOledb.AddParameter("@BookID", bookID);

            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                if (reader.Read())
                {
                    return Convert.ToString(reader["ReviewID"]);
                }
                return "0";
            }

        }

      
    }
}
