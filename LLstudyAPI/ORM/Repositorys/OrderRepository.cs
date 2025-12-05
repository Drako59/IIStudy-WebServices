using LLStudy_Models.Models;
using LLstudyWS.ORM.CreatorsModels;
using Microsoft.AspNetCore.Components.Web;
using System.Data;

namespace LLstudyWS.ORM
{
    public class OrderRepository : Repository<Order>, IRepository<Order>
    {
        public OrderRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }


        public bool AddRealationsOfBooksAndOrder(string registerID, List<string> booksID)
        {
            string sql = "INSERT INTO Orders_Books "
        }

        public List<Order> GetUserOrders(string ID)
        {
            List<Order> orders = new List<Order>();

            string sql = "SELECT * FROM Orders WHERE RegisteredID = @ID";
            this.helperOledb.AddParameter("@ID", ID);

            using(IDataReader reader = this.helperOledb.Select(sql))
            {
                while(reader.Read())
                {
                    orders.Add(this.moderlRefCreator.CreateModel<Order>(reader));
                }
            }
            return orders;
        }
    }
}
