using LLStudy_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.ViewModels
{
    public class ViewReview: Review
    {
        public string UserName { get; set; }
        public string ImagePath { get; set; }
        public string RegisteredID { get; set; }
    }
}
