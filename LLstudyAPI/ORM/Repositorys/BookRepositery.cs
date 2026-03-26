using LLStudy_Models;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using LLStudy_Models.ViewModels.Guest;
using LLstudyWS.ORM.CreatorsModels;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using static System.Reflection.Metadata.BlobBuilder;

namespace LLstudyWS.ORM

{
    public class BookRepository :Repository<Book>, IRepository<Book>
    {

        public BookRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }

       

        public List<Book> GetUserNameBooks(string RegisteredID)
        {
            List<Book> books = new List<Book>();

            string sql = $@"SELECT Books.BookID AS [BookID], * FROM  Books
                                    INNER JOIN (
                                        Orders_Books
                                        INNER JOIN Orders ON Orders.orderID = Orders_Books.OrderID
                                    ) ON Books.bookID = Orders_Books.BookID
                                WHERE
                                    Orders.RegisteredID = @RegisteredID";

            this.helperOledb.AddParameter("@RegisteredID", RegisteredID);
            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {
                    books.Add(this.moderlRefCreator.CreateModel<Book>(reader));
                }
            }
            return books;
        }

        public List<Book> GetByName(string name)
        {
            Console.WriteLine(">>>" + name + "<<<");
            string sql = $@"SELECT
                            *
                        FROM
                            Books
                        WHERE
                            book_name LIKE @Name  OR author_name LIKE  @Name";
            this.helperOledb.AddParameter("@Name","%" + name +"%");
            List<Book> books = new List<Book>();


            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {
                    books.Add(this.moderlRefCreator.CreateModel<Book>(reader));
                }

            }

            //foreach(Book book in books)
            //{
            //    Console.WriteLine($@"Book Name: {book.Book_name}");
            //}
            return books;

        }
        public int CountBookForUser(string BookID, string registeredID)
        {
            string sql = @$"SELECT 
                                CountBooks
                            FROM Shopping_carts
                            WHERE BookID = @BookID
                              AND RegisteredID = @RegisteredID;
                            ";
            this.helperOledb.AddParameter("@BookID", BookID);

            this.helperOledb.AddParameter("@RegisteredID", registeredID);



            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                if (reader.Read())
                {
                    return Convert.ToInt32(reader["CountBooks"]);
                }
                return -1;
            }
        }

        public List<CartBookViewModel> GetShoppingCartBooks(string userID)
        {
            string sql = @$"SELECT Books.BookID AS [BookID],
                            *
                        FROM
                            Registereds
                            INNER JOIN (
                                Books
                                INNER JOIN Shopping_carts ON Books.BookID = Shopping_carts.BookID
                            ) ON (
                                Shopping_carts.RegisteredID = Registereds.RegisteredID
                            )
                        WHERE
                            (Registereds.RegisteredID = @RegisteredID)";

            CartBookViewModel CartBook;
            
            List<CartBookViewModel> books = new List<CartBookViewModel>();
            this.helperOledb.AddParameter("@RegisteredID", userID);
            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                while(reader.Read())
                {
                    CartBook = new CartBookViewModel()
                    {
                        Book = this.moderlRefCreator.CreateModel<Book>(reader)
                        
                    };
                    CartBook.CountBooks = Convert.ToInt32(reader["CountBooks"]);
                    books.Add(CartBook);
                    
                }
            }
            return books;
        }


       public int BooksCount()
        {
            string sql = "SELECT Count(BookID) as BookCount FROM Books";
            int count;
            using(IDataReader reader = this.helperOledb.Select(sql)) 
            {
                if(reader.Read())
                {
                    return count = Convert.ToInt32(reader["BookCount"]);
                }
            }
            return -1;
        }

        public double GetBookRate(string bookID)
        {
            double sumRate;
            double devider;
            string sql = @$"SELECT Count(*) AS Devider, IIF(SUM(Rate) IS NULL, 0, SUM(Rate)) AS SumRate
                            FROM Reviews
                            WHERE BookID = @BookID;";
            this.helperOledb.AddParameter("@BookID", bookID);
            using(IDataReader reader = this.helperOledb.Select(sql)){
                if (reader.Read())
                {
                    sumRate = Convert.ToDouble(reader["SumRate"]);
                    devider = Convert.ToDouble(reader["Devider"]);
                }
                else return 0;
            }

            
                  
            if (devider == 0 || sumRate == 0) return 0;
            return Math.Round(sumRate / devider,1);
        }
            


        public string ChangeImage(IFormFile file, string bookID)
        {

           
            if (file == null || file.Length == 0)
                throw new Exception("Empty file");

            //NEED TO ADD**********************************************************************
            //Registered reg2 = this.GetByID(registeredID);
            //File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "RegisteredImages",reg2.ImagePath));

            //string path = Path.Combine(Directory.GetCurrentDirectory()!, "App_Data","RegisteredsImages");
            string path = Path.Combine(Directory.GetCurrentDirectory()!, "wwwroot", "Images", "BooksImages");

            Directory.CreateDirectory(path);

            string ext = Path.GetExtension(file.FileName);
            //Console.WriteLine($"FileName = '{file.FileName}', ContentType = '{file.ContentType}'");

            if (string.IsNullOrEmpty(ext))
            {
                ext = (file.ContentType ?? "").ToLowerInvariant() switch
                {
                    "image/jpeg" => ".jpg",
                    "image/png" => ".png",
                    "image/gif" => ".gif",
                    _ => throw new Exception("Unsupported file type")
                };
            }

            string fileName = $"Book{bookID}{ext}";

            path = Path.Combine(path, fileName);
            Console.WriteLine("********************************" + path);


            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }



            return fileName;
        }

        public string ChangeFile(IFormFile file, string bookID)
        {


            if (file == null || file.Length == 0)
                throw new Exception("Empty file");

            //NEED TO ADD**********************************************************************
            //Registered reg2 = this.GetByID(registeredID);
            //File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "RegisteredImages",reg2.ImagePath));

            //string path = Path.Combine(Directory.GetCurrentDirectory()!, "App_Data","RegisteredsImages");
            string path = Path.Combine(Directory.GetCurrentDirectory()!, "wwwroot", "Files", "BooksFiles");

            Directory.CreateDirectory(path);

            string ext = Path.GetExtension(file.FileName);
            //Console.WriteLine($"FileName = '{file.FileName}', ContentType = '{file.ContentType}'");

            if (string.IsNullOrEmpty(ext))
            {
                ext = (file.ContentType ?? "").ToLowerInvariant() switch
                {
                    "application/pdf" => ".pdf",
                    _ => throw new Exception("Unsupported file type")
                };
            }

            string fileName = $"Book{bookID}{ext}";

            path = Path.Combine(path, fileName);
            Console.WriteLine("********************************" + path);


            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }



            return fileName;
        }

        public ViewBookViewModel GetFullBook(string bookID)
        {
            ViewBookViewModel book = new ViewBookViewModel();

            string sql = $@"SELECT * FROM Books";

            sql = $@"SELECT
                    b.BookID,
                    b.author_name AS Author_name,
                    b.book_name AS Book_name,
                    b.in_stock AS In_stock,
                    b.SubjectID,
                    b.book_price AS Book_price,
                    b.pdf_url_book AS Pdf_url_book,
                    b.BookImagePath,
                    b.IsDeleted,
                    s.Subject_name,
                    COUNT(r.ReviewID) AS ReviewsNumber,
                    IIF(COUNT(r.ReviewID) = 0, 0, ROUND(AVG(r.Rate), 1)) AS Rate
                FROM
                    (
                        Books b
                        LEFT JOIN Subjects s ON b.SubjectID = s.SubjectID
                    )
                    LEFT JOIN Reviews r ON b.BookID = r.BookID
                WHERE b.BookID = @BookID
                GROUP BY
                    b.BookID,
                    b.author_name,
                    b.book_name,
                    b.in_stock,
                    b.SubjectID,
                    b.book_price,
                    b.pdf_url_book,
                    b.BookImagePath,
                    b.IsDeleted,
                    s.Subject_name";
            this.helperOledb.AddParameter("@BookID", bookID);
                
            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                if (reader.Read())
                {
                    book = this.moderlRefCreator.CreateModel<ViewBookViewModel>(reader, exludes: new List<string> { "book", "reviews" });
                    book.book = this.moderlRefCreator.CreateModel<Book>(reader);

                    

                }
                else  
                    return null;
            }

            
            //book.Rate = this.GetBookRate(book.book.BookID);
            book.reviews = this.GetReviewsByBook(book.book.BookID);
            //book.reviewsNumber = book.reviews.Count();
            return book;
        }
        public List<ViewBookViewModel> GetFullBooks()
        {
            List<ViewBookViewModel> books = new List<ViewBookViewModel>();

            string sql = $@"SELECT * FROM Books";

            sql = $@"SELECT
                    b.BookID,
                    b.author_name AS Author_name,
                    b.book_name AS Book_name,
                    b.in_stock AS In_stock,
                    b.SubjectID,
                    b.book_price AS Book_price,
                    b.pdf_url_book AS Pdf_url_book,
                    b.BookImagePath,
                    b.IsDeleted,
                    s.Subject_name,
                    COUNT(r.ReviewID) AS ReviewsNumber,
                    IIF(COUNT(r.ReviewID) = 0, 0, ROUND(AVG(r.Rate), 1)) AS Rate
                FROM
                    (
                        Books b
                        LEFT JOIN Subjects s ON b.SubjectID = s.SubjectID
                    )
                    LEFT JOIN Reviews r ON b.BookID = r.BookID
                GROUP BY
                    b.BookID,
                    b.author_name,
                    b.book_name,
                    b.in_stock,
                    b.SubjectID,
                    b.book_price,
                    b.pdf_url_book,
                    b.BookImagePath,
                    b.IsDeleted,
                    s.Subject_name;";
            ViewBookViewModel model;
            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {
                    model = this.moderlRefCreator.CreateModel<ViewBookViewModel>(reader,exludes : new List<string> { "book" , "reviews" });
                    model.book = this.moderlRefCreator.CreateModel<Book>(reader);
                    
                    books.Add(model);
                    
                }
            }

            foreach(ViewBookViewModel book in books)
            {
                //book.Rate = this.GetBookRate(book.book.BookID);
                book.reviews = this.GetReviewsByBook(book.book.BookID);
                //book.reviewsNumber = book.reviews.Count();
            }
            return books;
        }

        public List<BookShownDesktop> GetDesktopBooks()
        {
            List<BookShownDesktop> books = new List<BookShownDesktop>();

            string sql = $@"SELECT * FROM Books";

            sql = $@"SELECT
                    b.BookID,
                    b.author_name AS Author_name,
                    b.book_name AS Book_name,
                    b.in_stock AS In_stock,
                    b.SubjectID,
                    b.book_price AS Book_price,
                    b.pdf_url_book AS Pdf_url_book,
                    b.BookImagePath,
                    b.IsDeleted,
                    s.Subject_name,
                    COUNT(r.ReviewID) AS ReviewsNum,
                    IIF(COUNT(r.ReviewID) = 0, 0, ROUND(AVG(r.Rate), 1)) AS Rate
                FROM
                    (
                        Books b
                        LEFT JOIN Subjects s ON b.SubjectID = s.SubjectID
                    )
                    LEFT JOIN Reviews r ON b.BookID = r.BookID
                GROUP BY
                    b.BookID,
                    b.author_name,
                    b.book_name,
                    b.in_stock,
                    b.SubjectID,
                    b.book_price,
                    b.pdf_url_book,
                    b.BookImagePath,
                    b.IsDeleted,
                    s.Subject_name;";

            BookShownDesktop model;
            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {
                    model = this.moderlRefCreator.CreateModel<BookShownDesktop>(reader); //, exludes : new List<string>() { "reviewsNum","Rate" }

                    books.Add(model);

                }
            }

            //foreach (BookShownDesktop book in books)
            //{
            //    book.Rate = this.GetBookRate(book.BookID);
            //    book.reviewsNum = this.reviewNumber(book.BookID);
                
            //}
            return books;
        }

        public int reviewNumber(string bookID)
        {
            int counter;
            string sql = @$"SELECT Count(*) AS counterReviews
                            FROM Reviews
                            WHERE BookID = @BookID;";

            this.helperOledb.AddParameter("@BookID", bookID);
            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                if (reader.Read())
                {
                    counter = Convert.ToInt32(reader["counterReviews"]);
                }
                else return 0;
                return counter;
            }
        }

        public List<Book> GetExistBooks()
        {
            List<Book> books = new List<Book>();
            string sql = "SELECT * FROM Books WHERE IsDeleted = False";
            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {

                    books.Add(this.moderlRefCreator.CreateModel<Book>(reader));

                };
                
            }
            return books;
        }
    }
}
