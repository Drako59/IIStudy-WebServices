using LLStudy_Models.Models;
using LLstudyWS.ORM.CreatorsModels;
using System.Data;

namespace LLstudyWS.ORM
{
    public class SolutionRepository : Repository<Solution>, IRepository<Solution>
    {
        public SolutionRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }

        public List<Solution> GetByYear(string year)
        {
            string sql = $@"SELECT * FROM Solutions WHERE Solution_Year = @year";
            this.helperOledb.AddParameter("@year", year);
            List<Solution> solutions = new List<Solution>();
            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {
                    solutions.Add(this.moderlRefCreator.CreateModel<Solution>(reader));
                }
            }
            return solutions;
        }

        public List<Solution> GetBySubjectId(string subjectID)
        {
            string sql = $@"SELECT * FROM Solutions WHERE CategoryID = @subjectID";

            this.helperOledb.AddParameter("@subjectID", subjectID);
            List<Solution> solutions = new List<Solution>();
            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {
                    solutions.Add(this.moderlRefCreator.CreateModel<Solution>(reader));
                }
            }
            return solutions;

        }
    }
}
