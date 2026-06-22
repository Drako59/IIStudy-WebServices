using LLStudy_Models.Models;
using LLStudy_Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.ViewModels
{
    public class RegisteredComments : Book
    {
        string reviewID;
        double rate;
        string comment;
        //[IsDigits(ErrorMessage = "ID must contain only digits.")]
        //[Required]
        public string ReviewID { get { return reviewID; } set { reviewID = value; } }
        public double Rate { get { return rate; } set { rate = value; } }

        public string Comment { get { return comment; } set { comment = value; } }

        public string RegisteredID { get; set; }

        public int LikesCount { get; set; }
        public int Dislikes { get; set; }
    }
}
