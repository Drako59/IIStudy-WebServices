using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
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
        public double Rate { get; set; }
        public Book book { get; set; }

        public string Subject_name { get; set; }
        public List<ViewReview> reviews { get; set; }
        public int reviewsNumber { get; set; }
    }
}
