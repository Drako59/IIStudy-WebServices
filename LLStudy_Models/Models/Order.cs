using LLStudy_Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LLStudy_Models.Models
{
    public class Order: Model
    {
        string order_ID;
        string user_name;
        bool delivered;
        string location;
        double total_price;
        //[IsDigits(ErrorMessage = "ID must contain only digits.")]
        //[Required]
        public string Order_ID { get { return order_ID; } set { order_ID = value; } }
        [Required]
        public string User_name { get { return user_name; } set { user_name = value; } }
        [Required]
        public bool Delivered { get { return delivered; } set { delivered = value; } }

        public string Location { get { return location; } set { location = value; } }
        [Required]
        public double Total_price { get { return total_price; } set { total_price = value; } }


    }
}
