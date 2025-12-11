using LLStudy_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.ViewModels
{
    public class ViewBookViewModel
    {
        public Book book { get; set; }
        public List<Review> reviews { get; set; }
    }
}
