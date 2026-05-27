using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LLStudy_Models.Models;
namespace LLStudy_Models.ViewModels
{
    public class ViewShoppingCartModel
    {
        public Registered User { get; set; }
        public List<CartBookViewModel> CartBooks { get; set; }
        public List<string> OutOfStockBooks { get; set; }
    }
}
