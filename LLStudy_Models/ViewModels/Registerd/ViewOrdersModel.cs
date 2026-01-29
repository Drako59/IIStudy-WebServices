using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels.Registerd;


namespace LLStudy_Models.ViewModels
{
    public class ViewOrdersModel
    {
        public Registered User { get; set; }
        //public List<Order> Orders { get; set; }
        public List<ViewOrderDetailsModel> Orders { get; set; }
    }
}
