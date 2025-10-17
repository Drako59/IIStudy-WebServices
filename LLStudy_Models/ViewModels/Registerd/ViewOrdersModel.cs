using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LLStudy_Models.Model


namespace LLStudy_Models.ViewModels.Registerd
{
    public class ViewOrdersModel
    {
        Registered User { get; set; }
        public List<Order> Orders { get; set; }
    }
}
