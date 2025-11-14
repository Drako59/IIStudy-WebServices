using LLStudy_Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LLStudy_Models.Models
{
    public class Exam: Model
    {
      
        string examID;
        string categoryID;
        bool access;
        string file_path_url;
        string exam_Name;
        string exam_Year;
        //[Required]
        //[IsDigits(ErrorMessage = "ID must contain only digits.")]
        public string ExamID { get { return examID; } set { examID = value; } }
        [Required]
        public string CategoryID { get { return categoryID; } set { categoryID = value; } }
        
        public bool Access { get { return access; } set { access = value; } }
        [Required]
        public string File_path_url { get { return file_path_url; } set { file_path_url = value; } }
        [Required]
        public string Exam_Name { get { return exam_Name; } set { exam_Name = value; } }
        public string Exam_Year { get { return exam_Year; } set { exam_Year = value; } }


    }
}
