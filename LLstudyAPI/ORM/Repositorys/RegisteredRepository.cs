using LLStudy_Models.Models;
using LLstudyWS.ORM.CreatorsModels;
using System.Data;

namespace LLstudyWS.ORM
{
    public class RegisteredRepository : Repository<Registered>, IRepository<Registered>
    {
        public RegisteredRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }

        public string Login(string username, string password)
        {
            string sql = "SELECT password FROM Registers WHERE UserName = @USERNAME AND password = @PASSWORD";
            this.helperOledb.AddParameter("@USERNAME", username);
            this.helperOledb.AddParameter("@PASSWORD", password);
            using (IDataReader reader = this.helperOledb.Select(sql)) 
            {
                if (reader.Read())
                    return reader["UserName"].ToString();
            }
            return null;


        }
        public bool Create()
        {
            throw new NotImplementedException();
        }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public List<Registered> GetAll()
        {
            throw new NotImplementedException();
        }

        public Registered GetByID(string ID)
        {
            throw new NotImplementedException();
        }

        public bool Update()
        {
            throw new NotImplementedException();
        }

        public bool Create(Registered model)
        {
            throw new NotImplementedException();
        }

        public bool Update(Registered model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
