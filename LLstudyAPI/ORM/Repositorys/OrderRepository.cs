using LLStudy_Models.Models;
using LLstudyWS.ORM.CreatorsModels;

namespace LLstudyWS.ORM
{
    public class OrderRepository : Repository<Order>, IRepository<Order>
    {
        public OrderRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }

        public bool Create()
        {
            throw new NotImplementedException();
        }

        public bool Create(Order model)
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

        public List<Order> GetAll()
        {
            throw new NotImplementedException();
        }

        public Order GetByID(string ID)
        {
            throw new NotImplementedException();
        }

        public bool Update()
        {
            throw new NotImplementedException();
        }

        public bool Update(Order model)
        {
            throw new NotImplementedException();
        }
    }
}
