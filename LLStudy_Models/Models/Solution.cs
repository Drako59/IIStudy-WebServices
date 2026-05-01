using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using LLStudy_Models.Validation;
namespace LLStudy_Models.Models
{
    public class Solution: Model
    {
        string solution_ID;
        //string subjectID;
        string exam_ID;
        bool access;
        string file_path_url;
        string solution_Name;
        string solution_Year;

        public string SolutionID { get { return solution_ID; } set { solution_ID = value; } }
        
        [Required]
        [IsDigits(ErrorMessage = "ID must be a numebr.")]

        public string ExamID { get { return exam_ID; } set { exam_ID = value; } }
        public bool Access { get { return access; } set { access = value; } }
        [Required]

        public string File_path_url { get { return file_path_url; } set { file_path_url = value; } }
        [Required]

        public string Solution_Name { get { return solution_Name; } set { solution_Name = value; } }
        public string Solution_Year { get { return solution_Year; } set { solution_Year = value; } }



        //[Required]
        //[IsDigits(ErrorMessage = "ID must be a numebr.")]
        //public string CategoryID { get { return subjectID; } set { subjectID = value; } }
    }
}
