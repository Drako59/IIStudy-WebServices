using LLStudy_Models.Models;
using System.Data;

namespace LLstudyWS.ORM
{
    public class OrderCreator: IModelCreator<Order>
    {
        public Order CreateModel(IDataReader dataReader) { return new Order() { 
            Order_ID = Convert.ToString(dataReader["OrderID"]),
            User_name = Convert.ToString(dataReader["UserName"]),
            Delivered = Convert.ToBoolean(dataReader["delivered"]),
            Location = Convert.ToString(dataReader["location"]),
            Total_price = Convert.ToDouble(dataReader["Total_price"])

        }; }  
    }
}
