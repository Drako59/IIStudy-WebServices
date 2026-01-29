using LLStudy_Models.Models;
using LLstudyWS.ORM.CreatorsModels;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Data;

namespace LLstudyWS.ORM
{
    public class OrderRepository : Repository<Order>, IRepository<Order>
    {
        public OrderRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }

        //NOT IN USE
        public bool AddRealationOfBooksAndOrder(string orderID, string bookID)
        {
            string sql = @$"INSERT INTO Orders_Books (OrderID, BookID) VALUES (@OrderID,@BookID)";


            this.helperOledb.AddParameter("@OrderID", orderID);
            this.helperOledb.AddParameter("@BookID", bookID);

            return this.helperOledb.Insert(sql) > 0;
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

        public List<OrderBook> GetOrderBooks(string orderID)
        {
            string sql = @$"SELECT Books.BookID AS [BookID],
                            *
                        FROM
                            Orders
                            INNER JOIN (
                                Books
                                INNER JOIN Orders_Books ON Books.BookID = Orders_Books.BookID
                            ) ON (
                                Orders_Books.OrderID = Orders.OrderID
                            )
                        WHERE
                            (Orders.OrderID = @OrderID)";

            List<OrderBook> books = new List<OrderBook>();

            this.helperOledb.AddParameter("@OrderID",orderID);
            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {
                    books.Add(this.moderlRefCreator.CreateModel<OrderBook>(reader));
                }
            }
            return books;


        }
    }
}
