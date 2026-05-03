using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LLStudy_Models.Models
{
    public class Subject: Model
    {
        string subjectID;
        string subject_name;
        public string SubjectID { get { return subjectID; } set { subjectID = value; } }
        
        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "Max subject name Length is 20.")]
        public string Subject_name { get { return subject_name; } set { subject_name = value; ValidateProperty(value, nameof(this.Subject_name)); } }
    }
}
