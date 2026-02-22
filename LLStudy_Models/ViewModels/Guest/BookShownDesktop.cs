using LLStudy_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.ViewModels.Guest
{
    public class BookShownDesktop : Book
    {
        public int reviewsNum {get; set;}
        public double Rate { get; set; }
    }
}
