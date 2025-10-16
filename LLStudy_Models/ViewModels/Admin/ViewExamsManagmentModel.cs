using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LLStudy_Models.Model;


namespace LLStudy_Models.ViewModels.Admin
{
    public class ViewExamsManagmentModel
    {
        List<Exam> Exams { get; set; }
        List<Solution> Solutions { get; set; }
    }
}
