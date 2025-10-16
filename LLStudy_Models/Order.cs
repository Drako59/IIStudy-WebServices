using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models
{
    public class Order
    {
        string order_ID;
        string user_name;
        bool delivered;
        string location;
        double total_price;
        public string Order_ID { get { return order_ID; } set { order_ID = value; } }
        public string User_name { get { return user_name; } set { user_name = value; } }
        public bool Delivered { get { return delivered; } set { delivered = value; } }
        public string Location { get { return location; } set { location = value; } }
        public double Total_price { get { return total_price; } set { total_price = value; } }


    }
}
