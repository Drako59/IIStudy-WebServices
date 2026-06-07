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
        string exam_ID;
        string file_path_url;
        string solution_Name;
        string solution_Year;

        public string SolutionID { get { return solution_ID; } set { solution_ID = value; } }
        
        [Required]
        [IsDigits(ErrorMessage = "ID must be a numebr.")]

        public string ExamID { get { return exam_ID; } set { exam_ID = value; ValidateProperty(value, nameof(this.ExamID)); } }
        [Required]
        [ValidFile(ErrorMessage = "File format isn't valid.")]
        public string File_path_url { get { return file_path_url; } set { file_path_url = value; ValidateProperty(value, nameof(this.File_path_url)); } }
        [Required]
        [StringLength(maximumLength: 255, ErrorMessage = "Max solution name Length is 255.")]
        public string Solution_Name { get { return solution_Name; } set { solution_Name = value; ValidateProperty(value, nameof(this.Solution_Name)); } }

        [StringLength(maximumLength: 10, ErrorMessage = "Max Date Length is 10.")]
        [ValidDate(ErrorMessage = "The date isn't valid.")]
        public string Solution_Year { get { return solution_Year; } set { solution_Year = value; ValidateProperty(value, nameof(this.Solution_Year)); } }





        
    }
}
