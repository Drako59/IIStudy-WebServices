using LLStudy_Models.Models;
using LLstudyWS.ORM.CreatorsModels;

namespace LLstudyWS.ORM
{
    public class SolutionRepository : Repository<Solution>, IRepository<Solution>
    {
        public SolutionRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }

      
    }
}
