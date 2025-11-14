using LLStudy_Models.Models;
using LLstudyWS.ORM.CreatorsModels;

namespace LLstudyWS.ORM
{
    public class ShoppingCartRepository : Repository<Shopping_Cart>, IRepository<Shopping_Cart>
    {
        public ShoppingCartRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }

        public bool Create()
        {
            throw new NotImplementedException();
        }

        public bool Create(Shopping_Cart model)
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

        public List<Shopping_Cart> GetAll()
        {
            throw new NotImplementedException();
        }

        public Shopping_Cart GetByID(string ID)
        {
            throw new NotImplementedException();
        }

        public bool Update()
        {
            throw new NotImplementedException();
        }

        public bool Update(Shopping_Cart model)
        {
            throw new NotImplementedException();
        }
    }
}
