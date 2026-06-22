using LLStudy_Models.Models;
using LLstudyWS.ORM.CreatorsModels;
using System.Data;

namespace LLstudyWS.ORM.Repositorys
{
    public class LikeRepository : Repository<Likes> ,IRepository<Likes>
    {

        public LikeRepository(DbHelperOledb helper, ModelCreatorReflection modelCretorRef) : base(helper, modelCretorRef) { }

        public string LikeExistForUserAndReview(string registeredID, string reviewID)
        {
            string sql = "SELECT LikeID  FROM Likes WHERE ReigsterdID = @RegisteredID AND ReviewID = @ReviewID";

            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                if (reader.Read())
                    return Convert.ToString(reader["LikeID"]);
                return "0";
            }
        }

    }
}
