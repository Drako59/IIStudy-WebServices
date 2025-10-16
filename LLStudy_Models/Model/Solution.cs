using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models
{
    public class Solution
    {
        string solution_ID;
        string catregoryID;
        string exam_ID;
        bool access;
        string file_path_url;
        string solution_name;
        string solution_year;

        public string Solution_ID { get { return solution_ID; } set { solution_ID = value; } }
        public string CatregoryID { get { return catregoryID; } set { catregoryID = value; } }
        public string Exam_ID { get { return Exam_ID; } set { Exam_ID = value; } }
        public bool Access { get { return access; } set { access = value; } }
        public string File_path_url { get { return file_path_url; } set { file_path_url = value; } }
        public string Solution_name { get { return solution_name; } set { solution_name = value; } }
        public string Solution_year { get { return solution_year; } set { solution_year = value; } }

    }
}
