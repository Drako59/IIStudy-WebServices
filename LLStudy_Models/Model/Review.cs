using LLStudy_Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models
{
    public class Review
    {

        string review_ID;
        string rate;
        string comment;
        string book_ID;
        string userName;
        //[IsDigits(ErrorMessage = "ID must contain only digits.")]
        //[Required]
        public string Review_ID { get { return review_ID; } set { review_ID = value; } }
        [IsDigits(ErrorMessage = "ID must contain only digits.")]
        [Required]
        public string Rate { get { return rate; } set { rate = value; } }

        public string Comment { get { return comment; } set { comment = value; } }
        [Required]
        public string Book_ID { get { return book_ID; } set { book_ID = value; } }
        [Required]
        public string UserName { get { return userName; } set { userName = value; } }

    }
}
