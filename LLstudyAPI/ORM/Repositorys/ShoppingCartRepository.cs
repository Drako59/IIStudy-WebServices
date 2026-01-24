using LLStudy_Models.Models;
using LLstudyWS.ORM.CreatorsModels;
using System.Data;

namespace LLstudyWS.ORM
{
    public class ShoppingCartRepository : Repository<Shopping_Cart>, IRepository<Shopping_Cart>
    {
        public ShoppingCartRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }

        
        public override Shopping_Cart GetByID(string RegisteredID, List<string>? exludes = null)
        {
            string sql = "SELECT * FROM Registereds WHERE RegisteredID = @RegisteredID";

            this.helperOledb.AddParameter("@RegisteredID", RegisteredID);

            Shopping_Cart obj;
            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                if (reader.Read())
                {
                    obj = this.moderlRefCreator.CreateModel<Shopping_Cart>(reader);
                    return obj;
                }
            }
            return new Shopping_Cart();
        }

        public bool RemoveBookForUser( string BookID, string registeredID)
        {
            Console.WriteLine(BookID + " "+registeredID);
            string sql = "DELETE * FROM Shopping_carts WHERE BookID = @BookID AND RegisteredID = @RegisteredID";
            this.helperOledb.AddParameter("@BookID", BookID);

            this.helperOledb.AddParameter("@RegisteredID", registeredID);

            return this.helperOledb.Delete(sql) > 0;


        }


       
        
    }
}
