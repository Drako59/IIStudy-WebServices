using LLStudy_Models.Models;
using LLstudyWS.ORM.CreatorsModels;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Data;
using System.Runtime.CompilerServices;

namespace LLstudyWS.ORM
{
    public class OrderRepository : Repository<Order>, IRepository<Order>
    {
        public OrderRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }

        
        public bool AddRealationOfBooksAndOrder(string orderID, string registeredID)
        {
            //string sql = @$"INSERT INTO Orders_Books (OrderID, BookID,Amount) VALUES (@OrderID,@BookID,@Amount)";

            string sql = $@"INSERT INTO Orders_Books (OrderID, BookID, Amount)
                    SELECT 
                        @OrderID,
                        BookID,
                        CountBooks
                    FROM Shopping_carts
                    WHERE RegisteredID = @RegisteredID";

            this.helperOledb.AddParameter("@OrderID", orderID);
            this.helperOledb.AddParameter("@RegisteredID", registeredID);

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

        public bool CheckIfBookExistForUser(string bookID, string registeredID)
        {
            string sql = $@"SELECT COUNT(*) AS BooksNum FROM  Books
                                    INNER JOIN (
                                        Orders_Books
                                        INNER JOIN Orders ON Orders.orderID = Orders_Books.OrderID
                                    ) ON Books.bookID = Orders_Books.BookID
                                WHERE
                                    Orders.RegisteredID = @RegisteredID AND Books.BookID = @BookID";

            this.helperOledb.AddParameter("@RegisteredID", registeredID);
            this.helperOledb.AddParameter("@BookID", bookID);

            using(IDataReader reader = this.helperOledb.Select(sql))
            {
                if (reader.Read())
                {
                    //Console.WriteLine("CountBooks Check-> " + Convert.ToInt32(reader["BooksNum"]));
                    return Convert.ToInt32(reader["BooksNum"]) >  0;
                }
                return false;
            }
        }

        
    }
}
