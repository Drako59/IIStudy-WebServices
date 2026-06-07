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
        string orderID;
        bool delivered;
        string location;
        double total_price;
        string registeredID;

        public string OrderID { get { return orderID; } set { orderID = value; } }

        [Required]
        public string Location { get { return location; } set { location = value; } }
        
        [Required]
        public double Total_price { get { return total_price; } set { total_price = value; } }
        
        public string RegisteredID { get; set; }

        [ValidDate(ErrorMessage = "The date isn't valid.")]
        public string Date { get; set; }

        [Required]
        public int DeliveryStatus { get; set; }

        [Postal(ErrorMessage = "Postal number isn't valid for ISRAEL.")]
        [Required]
        public string Postal { get; set; }

        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }



    }
}
