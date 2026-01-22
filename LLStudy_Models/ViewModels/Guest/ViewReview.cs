using LLStudy_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.ViewModels
{
    public class ViewReview
    {
        public string UserName { get; set; }
        public string ImagePath { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
        public string RegisteredID { get; set; }
        public string ReviewID { get; set; }
    }
}
