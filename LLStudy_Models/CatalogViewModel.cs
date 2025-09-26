using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models
{
    public class CatalogViewModel
    {
        public List<Book> Books { get; set; }
        public List<Order> Orders { get; set; }
        public List<Registered> registereds { get; set; }
    }
}
