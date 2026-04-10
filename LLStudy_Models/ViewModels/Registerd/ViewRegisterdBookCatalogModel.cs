using LLStudy_Models.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.ViewModels
{
    public class ViewRegisterdBookCatalogModel
    {
        public List<Book> books { get; set; } 
        public List<string> OwnedOnlineBooksIDs { get; set; }
        public List<string> OnlineBooksInShoppingCartIDs { get; set; }
    }
}
