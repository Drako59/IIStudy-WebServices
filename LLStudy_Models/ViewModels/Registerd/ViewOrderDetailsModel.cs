using LLStudy_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.ViewModels.Registerd
{
    public class ViewOrderDetailsModel
    {
        public List<Book> Books { get; set; }
        public Order Order { get; set; }
        public Registered Registered { get; set; }
    }
}
