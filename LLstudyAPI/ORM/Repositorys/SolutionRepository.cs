using LLStudy_Models.Models;
using LLstudyWS.ORM.CreatorsModels;

namespace LLstudyWS.ORM
{
    public class SolutionRepository : Repository<Solution>, IRepository<Solution>
    {
        public SolutionRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }

        public bool Create()
        {
            throw new NotImplementedException();
        }

        public bool Create(Solution model)
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

        public List<Solution> GetAll()
        {
            throw new NotImplementedException();
        }

        public Solution GetByID(string ID)
        {
            throw new NotImplementedException();
        }

        public bool Update()
        {
            throw new NotImplementedException();
        }

        public bool Update(Solution model)
        {
            throw new NotImplementedException();
        }
    }
}
