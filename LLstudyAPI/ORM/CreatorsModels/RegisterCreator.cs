using LLStudy_Models.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace LLstudyWS.ORM
{
    public class RegisterCreator: IModelCreator<Registered>
    {
        public Registered CreateModel(IDataReader dataReader)
        {
            return new Registered() {
                UserName = Convert.ToString(dataReader["UserName"]),
                Password = Convert.ToString(dataReader["password"]),
                Email = Convert.ToString(dataReader["email"]),
                Role = Convert.ToString(dataReader["role"]),
                Birth = Convert.ToString(dataReader["Birth"])
            };

        }
    }
}
