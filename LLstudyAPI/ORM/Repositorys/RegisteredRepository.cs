using LLStudy_Models.Models;
using LLstudyWS.ORM.CreatorsModels;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.CompilerServices;

namespace LLstudyWS.ORM
{
    public class RegisteredRepository : Repository<Registered>, IRepository<Registered>
    {
        public RegisteredRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }

        public Registered Login( string password, string? username = null, string? email = null )
            
        {
            
            string sql = "SELECT * FROM Registereds WHERE UserName = @USERNAME AND Password = @PASSWORD";
            if (email != null)
            {
                sql = "SELECT * FROM Registereds WHERE Email = @Email AND Password = @PASSWORD";
                this.helperOledb.AddParameter("@Email", email);

            }
            else if (username != null)
            {
                this.helperOledb.AddParameter("@USERNAME", username);

            }
            else
                return null;


                this.helperOledb.AddParameter("@PASSWORD", password);
            using (IDataReader reader = this.helperOledb.Select(sql)) 
            {
                if (reader.Read())
                    return this.moderlRefCreator.CreateModel<Registered>(reader, new List<string>() { "Password" });
            }
            return null;


        }

        public string LoginID(string password, string? username = null, string? email = null)
        {
            string sql = "SELECT * FROM Registereds WHERE UserName = @USERNAME AND Password = @PASSWORD";

            if (email != null)
            {
                sql = "SELECT * FROM Registereds WHERE Email = @Email AND Password = @PASSWORD";
                this.helperOledb.AddParameter("@Email", email);

            }
            else if (username != null)
            {
                this.helperOledb.AddParameter("@USERNAME", username);

            }
            else
                return null;


                this.helperOledb.AddParameter("@PASSWORD", password);
            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                if (reader.Read())
                    return Convert.ToString(reader["RegisteredID"]);
            }
            return null;
        }


        //public override Registered GetByID(string UserName)
        //{
        //    string sql = "SELECT * FROM Registereds WHERE UserName = @UserName";

        //    this.helperOledb.AddParameter("@UserName", UserName);

        //    Registered obj;
        //    using(IDataReader reader = this.helperOledb.Select(sql))
        //    {
        //        if (reader.Read())
        //        {
        //            obj = this.moderlRefCreator.CreateModel<Registered>(reader);
        //            return obj;
        //        }
        //    }
        //    return new Registered();
        //}
    }
}
