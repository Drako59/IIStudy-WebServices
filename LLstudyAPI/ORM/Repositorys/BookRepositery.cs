using LLStudy_Models;
using LLStudy_Models.Models;
using LLstudyWS.ORM.CreatorsModels;
using System.Data;
using System.Data.SqlTypes;
using System.Reflection.Metadata.Ecma335;

namespace LLstudyWS.ORM

{
    public class BookRepository :Repository<Book>, IRepository<Book>
    {

        public BookRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }

        //public override bool Create(Book model)
        //{
        //    this.helperOledb.OpenConnection();
        //    //string sql = $@"INSERT INTO Books (
        //    //                author_name,
        //    //                book_name,
        //    //                book_price,
        //    //                In_stock,
        //    //                subjectID,
        //    //                pdf_url_book
        //    //                )
        //    //            VALUES
        //    //                ('{model.Author_name}', '{model.Book_name}', {model.Book_price}, {model.In_stock}, {model.SubjectID}, '{model.Pdf_url_book}') ";
        //    string sql = $@"INSERT INTO Books (
        //                    author_name,
        //                    book_name,
        //                    book_price,
        //                    In_stock,
        //                    subjectID,
        //                    pdf_url_book
        //                    )
        //                VALUES
        //                    (@AuthorName, @BookName, @BookPrice, @InStock, @SubjectID, @PdfUrlBook) ";
        //    this.helperOledb.AddParameter("@AuthorName", model.Author_name);
        //    this.helperOledb.AddParameter("@BookName", model.Book_name);
        //    this.helperOledb.AddParameter("@BookPrice", model.Book_price.ToString());
        //    this.helperOledb.AddParameter("@InStock", model.In_stock.ToString());
        //    this.helperOledb.AddParameter("@SubjectID", model.SubjectID);
        //    this.helperOledb.AddParameter("@PdfUrlBook", model.Pdf_url_book);
        //    return this.helperOledb.Insert(sql) > 0;





        //}


        //public bool Delete(string id)
        //{
        //    string sql = "DELETE * FROM Books WHERE book_ID = @BookID";
        //    this.helperOledb.AddParameter("@BookID", id);
        //    return this.helperOledb.Delete(sql) > 0;
        //}

        //public List<Book> GetAll()
        //{
        //    string sql = "SELECT * FROM Books";
        //    List<Book> books = new List<Book>();

        //    using (IDataReader reader = this.helperOledb.Select(sql))
        //    {
        //        while (reader.Read())
        //        {
        //            books.Add(this.modelCreators.BookCreator.CreateModel(reader));
        //        }
        //    }
        //    return books;
        //}

        //public Book GetByID(string ID)
        //{
        //    string sql = @$"SELECT * FROM Books WHERE book_ID = {ID}";
        //    using (IDataReader reader = this.helperOledb.Select(sql)) 
        //    { 
        //        reader.Read();
        //        return this.modelCreators.BookCreator.CreateModel(reader);
        //    }
        //}

        public List<Book> GetUserNameBooks(string userName)
        {
            List<Book> books = new List<Book>();

            string sql = $@"SELECT     Books.BookID AS [BookID], * FROM  Books
                                    INNER JOIN (
                                        Orders_Books
                                        INNER JOIN Orders ON Orders.orderID = Orders_Books.OrderID
                                    ) ON Books.bookID = Orders_Books.BookID
                                WHERE
                                    Orders.UserName = @userName";

            this.helperOledb.AddParameter("@userName", userName);

            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {
                    books.Add(this.moderlRefCreator.CreateModel<Book>(reader));
                }
            }
            return books;
        }
       
    }
}
