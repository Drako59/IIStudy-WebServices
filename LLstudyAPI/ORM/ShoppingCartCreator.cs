using LLStudy_Models.Models;
using System.Data;

namespace LLstudyWS.ORM
{
    public class ShoppingCartCreator : IModelCreator<Shopping_Cart>
    {
        public Shopping_Cart CreateModel(IDataReader dataReader)
        {
            return new Shopping_Cart() 
            {
                UserName = Convert.ToString(dataReader["UserName"])
            };
        }
    }
}
