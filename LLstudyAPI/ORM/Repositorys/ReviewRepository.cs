using LLStudy_Models.Models;
using LLstudyWS.ORM.CreatorsModels;

namespace LLstudyWS.ORM
{
    public class ReviewRepository : Repository<Review>, IRepository<Review>
    {
        public ReviewRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }

        public bool Create()
        {
            throw new NotImplementedException();
        }

        public bool Create(Review model)
        {
            throw new NotImplementedException();
        }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public List<Review> GetAll()
        {
            throw new NotImplementedException();
        }

        public Review GetByID(string ID)
        {
            throw new NotImplementedException();
        }

        public bool Update()
        {
            throw new NotImplementedException();
        }

        public bool Update(Review model)
        {
            throw new NotImplementedException();
        }
    }
}
