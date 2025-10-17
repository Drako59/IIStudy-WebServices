using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LLStudy_Models.Models
{
    public class CategoryModel: Model
    {
        string subjectID;
        string subject_name;
        public string SubjectID { get { return subjectID; } set { subjectID = value; } }
        public string Subject_name { get { return subject_name; } set { subject_name = value; } }
    }
}
