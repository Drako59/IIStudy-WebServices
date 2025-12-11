using LLStudy_Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.Models
{
    public class Review:Model
    {

        string reviewID;
        string rate;
        string comment;
        string bookID;
        string userName;
        //[IsDigits(ErrorMessage = "ID must contain only digits.")]
        //[Required]
        public string ReviewID { get { return reviewID; } set { reviewID = value; } }
        [IsDigits(ErrorMessage = "ID must contain only digits.")]
        [Required]
        public string Rate { get { return rate; } set { rate = value; } }

        public string Comment { get { return comment; } set { comment = value; } }
        [Required]
        public string BookID { get { return bookID; } set { bookID = value; } }
        [Required]
        public string UserName { get { return userName; } set { userName = value; } }

    }
}
