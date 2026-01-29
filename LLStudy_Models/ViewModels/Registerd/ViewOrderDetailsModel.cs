using LLStudy_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.ViewModels
{
    public class ViewOrderDetailsModel
    {
        public List<OrderBook> Books { get; set; }
        public Order Order { get; set; }
    }
}
