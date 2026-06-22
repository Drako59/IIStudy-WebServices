using LLStudy_Models.Models;
using LLstudyWS.ORM.CreatorsModels;
using System.Data;

namespace LLstudyWS.ORM.Repositorys
{
    public class LikeRepository : Repository<Like> ,IRepository<Like>
    {

        public LikeRepository(DbHelperOledb helper, ModelCreatorReflection modelCretorRef) : base(helper, modelCretorRef) { }

        public string LikeExistForUserAndReview(string registeredID, string reviewID)
        {
            string sql = "SELECT LikeID  FROM Likes WHERE RegisteredID = @RegisteredID AND ReviewID = @ReviewID";
            this.helperOledb.AddParameter("@RegisteredID", registeredID);
            this.helperOledb.AddParameter("@ReviewID", reviewID);

            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                if (reader.Read())
                    return Convert.ToString(reader["LikeID"]);
                return "0";
            }
        }

        public List<string> GetReviewsLikedByUserForBook(string registeredID, string bookID)
        {
            string sql = @$"SELECT Reviews.ReviewID AS [ReviewID] FROM Books INNER JOIN
                                                (
                                                    Reviews INNER JOIN Likes ON Reviews.ReviewID = Likes.ReviewID
                                                ) ON Books.BookID = Reviews.BookID
                                            WHERE  Books.BookID = @BookID AND Likes.RegisteredID = @RegisteredID AND Likes.IsLike = True";
            this.helperOledb.AddParameter("@BookID", bookID);
            this.helperOledb.AddParameter("@RegisteredID", registeredID);

            using(IDataReader reader = this.helperOledb.Select(sql))
            {
                List<string> reviewsID = new List<string>();
                while (reader.Read())
                {
                    reviewsID.Add(Convert.ToString(reader["ReviewID"]));
                }
                return reviewsID;
            }

        }

        public List<string> GetReviewsDislikedByUserForBook(string registeredID, string bookID)
        {
            string sql = @$"SELECT Reviews.ReviewID AS [ReviewID] FROM Books INNER JOIN
                                                (
                                                    Reviews INNER JOIN Likes ON Reviews.ReviewID = Likes.ReviewID
                                                ) ON Books.BookID = Reviews.BookID
                                            WHERE  Books.BookID = @BookID AND Likes.RegisteredID = @RegisteredID AND Likes.IsDislike = True";
            this.helperOledb.AddParameter("@BookID", bookID);
            this.helperOledb.AddParameter("@RegisteredID", registeredID);

            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                List<string> reviewsID = new List<string>();
                while (reader.Read())
                {
                    reviewsID.Add(Convert.ToString(reader["ReviewID"]));
                }
                return reviewsID;
            }

        }
    }
}
