using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LLStudy_Models.Models;

namespace LLStudy_Models.ViewModels.Registerd
{
    internal class ViewOwnedBooksModel
    {
        Registered User;
        List<Book> Books { get; set; }
    }
}
