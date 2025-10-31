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
      
        string exam_ID;
        string catregoryID;
        bool access;
        string file_path_url;
        string exam_name;
        string exam_year;
        //[Required]
        //[IsDigits(ErrorMessage = "ID must contain only digits.")]
        public string Exam_ID { get { return exam_ID; } set { exam_ID = value; } }
        [Required]
        public string CatregoryID { get { return catregoryID; } set { catregoryID = value; } }
        
        public bool Access { get { return access; } set { access = value; } }
        [Required]
        public string File_path_url { get { return file_path_url; } set { file_path_url = value; } }
        [Required]
        public string Exam_name { get { return exam_name; } set { exam_name = value; } }
        public string Exam_year { get { return exam_year; } set { exam_year = value; } }


    }
}
