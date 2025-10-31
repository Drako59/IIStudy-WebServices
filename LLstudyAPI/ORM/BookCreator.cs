using LLStudy_Models;
using LLStudy_Models.Models;
using System.Data;

namespace LLstudyWS.ORM
{
    public class BookCreator: IModelCreator<Book>
    {
        public Book CreateModel(IDataReader dataReader)
        {
            
            Book book = new Book() {
                Book_name = Convert.ToString(dataReader["book_name"]),
                Book_price = Convert.ToDouble(dataReader["book_price"]),
                Author_name = Convert.ToString(dataReader["author_name"]),
                Book_ID = Convert.ToString(dataReader["book_ID"]),
                Pdf_url_book = Convert.ToString(dataReader["pdf_url_book"]),
                SubjectID = Convert.ToString(dataReader["subjectID"]),
                In_stock = Convert.ToBoolean(dataReader["In_stock"]),
        };
            //book.Book_name = Convert.ToString(dataReader["book_name"]);
            //book.Book_price = Convert.ToDouble(dataReader["book_price"]);
            //book.Author_name = Convert.ToString(dataReader["author_name"]);
            //book.Book_ID = Convert.ToString(dataReader["book_ID"]);
            //book.Pdf_url_book = Convert.ToString(dataReader["pdf_url_book"]);
            //book.SubjectID = Convert.ToString(dataReader["subjectID"]);
            //book.In_stock = Convert.ToBoolean(dataReader["In_stock"]);
            return book;

        }

        public Book CreateModel(IDataReader dataReader)
        {
            throw new NotImplementedException();
        }
    }
}
