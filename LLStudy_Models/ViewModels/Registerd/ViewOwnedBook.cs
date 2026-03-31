using LLStudy_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.ViewModels
{
    public class ViewOwnedBook : Book
    {
        public double AvgRate { get; set; }

        public string Subject_name { get; set; }
    }
}
