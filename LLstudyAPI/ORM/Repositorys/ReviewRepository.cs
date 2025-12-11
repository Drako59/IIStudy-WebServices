using LLStudy_Models.Models;
using LLstudyWS.ORM.CreatorsModels;
using System.Data;

namespace LLstudyWS.ORM
{
    public class ReviewRepository : Repository<Review>, IRepository<Review>
    {
        public ReviewRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }

        public List<Review> GetReviewsByBook(string bookID)
        {
            List<Review> reviews = new List<Review>();
            string sql = "SELECT * FROM Reviews WHERE BookID = @BookID";
            this.helperOledb.AddParameter("@BookID",bookID);

            using(IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {
                    reviews.Add(this.moderlRefCreator.CreateModel<Review>(reader));
                }
            }
            return reviews;
        }
    }
}
