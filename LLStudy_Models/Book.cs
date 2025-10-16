using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models
{
    public class Book
    {
        string book_ID;
        string author_name;
        double book_price;
        string book_name;
        string in_stock;
        string subjectID;
        string pdf_url_book;
        short string name;
        public string Book_ID {  get { return book_ID; } set {  book_ID = value; } }
        [Required(ErrorMessage = "Name was not set")]
        [StringLength(20,MinimumLength = 2, ErrorMessage ="Lenght is not between 2 and 20 characters.")]
        [FirstLetterCapital(ErrorMessage = "First letter isn't capital.")]
        public string Author_name { get { return author_name; } set { author_name = value; } }
        public string Book_name { get { return book_name; } set { book_name = value; } }
        public string In_stock { get { return in_stock; } set { in_stock = value; } }
        public string SubjectID { get { return subjectID; } set { subjectID = value; } }
        public double Book_price { get { return book_price; } set { book_price = value; } }
        public string Pdf_url_book { get { return pdf_url_book; } set { pdf_url_book = value; } }


    }
}
