using LLStudy_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.ViewModels
{
    public class ViewBookCatalogModel
    {
        public List<GuestBookDetails> Books { get; set; }
        public List<Subject> Subjects { get; set; }
        public int PageNumber { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public string? SubjectID { get; set; }
        public bool In_stock { get; set; }
        public bool IsOnline { get; set; }
        public bool IsPhysical { get; set; }
    }
}
