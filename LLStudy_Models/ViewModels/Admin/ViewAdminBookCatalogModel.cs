using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LLStudy_Models.Models;

namespace LLStudy_Models.ViewModels
{
    public   class ViewAdminBookCatalogModel
    {
        Registered User { get; set; }
        List<Book> Registereds { get; set; }
    }
}
