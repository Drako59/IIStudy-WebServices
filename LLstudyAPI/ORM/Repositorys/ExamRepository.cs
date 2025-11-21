using LLStudy_Models.Models;
using LLstudyWS.ORM.CreatorsModels;
using System.Data;

namespace LLstudyWS.ORM
{
    public class ExamRepository : Repository<Exam>, IRepository<Exam>
    {
        public ExamRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }


    }
}
