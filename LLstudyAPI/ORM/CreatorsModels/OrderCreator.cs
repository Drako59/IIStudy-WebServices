using LLStudy_Models.Models;
using System.Data;

namespace LLstudyWS.ORM
{
    public class OrderCreator: IModelCreator<Order>
    {
        public Order CreateModel(IDataReader dataReader)
        {
            return new Order()
            {
                OrderID = Convert.ToString(dataReader["OrderID"]),
                RegisteredID = Convert.ToString(dataReader["RegisteredID"]),
                //Delivered = Convert.ToBoolean(dataReader["Delivered"]),
                Location = Convert.ToString(dataReader["Location"]),
                Total_price = Convert.ToDouble(dataReader["Total_price"])

            };
        }
    }
}
