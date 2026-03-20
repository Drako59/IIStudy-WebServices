using LLStudy_Models.Models;
using LLstudyWS.ORM.CreatorsModels;
using System.Data;

namespace LLstudyWS.ORM
{
    public class SubjectRepository : Repository<Subject>, IRepository<Subject>
    {
        public SubjectRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }

        public Dictionary<string,string> GetSubjectsDict()
        {
            string sql = "SELECT * FROM Subjects";
            Dictionary<string, string> subjectsDict = new Dictionary<string, string>();
            List<Subject> subjects = new List<Subject>();
            using(IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {
                    subjects.Add(this.moderlRefCreator.CreateModel<Subject>(reader));
                }

                foreach( Subject subject in subjects)
                {
                    subjectsDict.Add(subject.SubjectID, subject.Subject_name);
                }
            }
            return subjectsDict;
        }


        public List<string> GetSubjectsNamesList()
        {
            string sql = "SELECT * FROM Subjects";
            List<string> names = new List<string>();

            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {
                    names.Add(Convert.ToString(reader["Subject_name"]));
                }

            }
            return names;
        }
    }
}
