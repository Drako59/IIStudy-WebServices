using LLStudy_Models.Models;
using LLstudyWS.ORM.CreatorsModels;
using System.Data;

namespace LLstudyWS.ORM
{
    public class RegisteredRepository : Repository<Registered>, IRepository<Registered>
    {
        public RegisteredRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }

        public Registered Login(string username, string password)
        {
            string sql = "SELECT * FROM Registereds WHERE UserName = @USERNAME AND Password = @PASSWORD";
            this.helperOledb.AddParameter("@USERNAME", username);
            this.helperOledb.AddParameter("@PASSWORD", password);
            using (IDataReader reader = this.helperOledb.Select(sql)) 
            {
                if (reader.Read())
                    return this.moderlRefCreator.CreateModel<Registered>(reader, new List<string>() { "Password" });
            }
            return null;


        }

        public override Registered GetByID(string UserName)
        {
            string sql = "SELECT * FROM Registereds WHERE UserName = @UserName";

            this.helperOledb.AddParameter("@UserName", UserName);

            Registered obj;
            using(IDataReader reader = this.helperOledb.Select(sql))
            {
                if (reader.Read())
                {
                    obj = this.moderlRefCreator.CreateModel<Registered>(reader);
                    return obj;
                }
            }
            return new Registered();
        }
    }
}
