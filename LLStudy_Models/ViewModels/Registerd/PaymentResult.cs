using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.ViewModels
{
    public class PaymentResult
    {
        public bool Success { get; set; }
        public bool CartIsEmpty { get; set; }
        public bool OutOfStockBooks { get; set; } 
        
    }
}
