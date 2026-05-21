using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.ViewModels
{
    public class SolutionDetailsWeb : SolutionDetails
    {
        public bool HasFile { get; set; }
        public string Exam_Name { get; set; }
        public string SubjectID { get; set; }
        public string Exam_Year { get; set; }
    }
}
