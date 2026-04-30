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
        string subjectID;
        bool access;
        string file_path_url;
        string exam_Name;
        string exam_Year;
        //[Required]
        //[IsDigits(ErrorMessage = "ID must contain only digits.")]
        public string ExamID { get { return examID; } set { examID = value; } }
        [Required]
        public string SubjectID { get { return subjectID; } set { subjectID = value; ValidateProperty(value, "SubjectID"); } }
        
        public bool Access { get { return access; } set { access = value; } }
        [Required]
        [ValidFile(ErrorMessage ="File format isn't valid.")]
        public string File_path_url { get { return file_path_url; } set { file_path_url = value; ValidateProperty(value, "File_path_url"); } }
        [Required]
        [StringLength(maximumLength: 255, ErrorMessage = "Max Exam_name Length is 255.")]

        public string Exam_Name { get { return exam_Name; } set { exam_Name = value; ValidateProperty(value, "Exam_Name"); } }
        [StringLength(maximumLength: 10, ErrorMessage = "Max Date Length is 10.")]
        [ValidDate(ErrorMessage ="The date isn't valid.")]
        public string Exam_Year { get { return exam_Year; } set { exam_Year = value; ValidateProperty(value, "Exam_Year"); } }

        public bool IsDeleted { get; set; }
    }
}
