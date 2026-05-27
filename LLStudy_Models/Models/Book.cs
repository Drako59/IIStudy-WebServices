using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LLStudy_Models.Validation;
using LLStudy_Models.Validation;   

namespace LLStudy_Models.Models
{
    public class Book: Model
    {
       
        string book_ID;
        string author_name;
        double book_price;
        string book_name;
        bool in_stock;
        string subjectID;
        string pdf_url_book;
        string bookImagePath;
        string type;
        string bookDetails;
        int stockCount;
        //[Required]
        [IsDigits(ErrorMessage = "ID must contain only digits.")]
        public string BookID {  get { return book_ID; } set {  book_ID = value; } }
        [Required(ErrorMessage = "Name was not set")]
        [StringLength(20,MinimumLength = 2, ErrorMessage ="Lenght is not between 2 and 20 characters.")]
        //[FIrstCapitalChar(ErrorMessage = "First letter isn't capital.")]
        public string Author_name { get { return author_name; } set { author_name = value; ValidateProperty(value, "Author_name"); } }
        [Required]
        [StringLength(maximumLength: 255, ErrorMessage = "Max Book_name Length is 255.")]

        public string Book_name { get { return book_name; } set { book_name = value; ValidateProperty(value, "Book_name"); } }
        public bool In_stock { get { return in_stock; } set { in_stock = value; } }
        [Required]
        public string SubjectID { get { return subjectID; } set { subjectID = value; ValidateProperty(value, "SubjectID"); } }
        [Required]
        [IsDigits(ErrorMessage = "ID must contain only digits.")]

        public double Book_price { get { return book_price; } set { book_price = value; ValidateProperty(value, "Book_price"); } }
        
        [Required]
        [ValidFile(ErrorMessage = "File format isn't valid.")]
        public string Pdf_url_book { get { return pdf_url_book; } set { pdf_url_book = value; ValidateProperty(value, "Pdf_url_book"); } }
        
        [StringLength(maximumLength: 255, ErrorMessage = "Max Book_name Length is 255.")]
        public string BookDetails { get { return bookDetails; } set {bookDetails = value; ValidateProperty(value, "BookDetails"); } }

        public int StockCount { get { return this.stockCount;} set { this.stockCount = value; ValidateProperty(value, nameof(this.StockCount)); } }
        public string BookImagePath { get {return bookImagePath; } set {bookImagePath = value; ValidateProperty(value, "BookImagePath"); } }

        public bool IsDeleted { get; set; }

        public bool IsOnline { get; set; }

    }
}
