using IIStudyDESKTOP.WindowsPages;
using IIstudyWSClient;
using LLStudy_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace IIStudyDESKTOP.WindowsPages
{
    /// <summary>
    /// Interaction logic for BookDetails.xaml
    /// </summary>
    public partial class BookDetails : Window
    {
        EditBook editBook;
        private Book book;
        public BookDetails(Book book)
        {
            this.book = book;
            InitializeComponent();
            LoadBook();
        }

        private void LoadBook()
        {
            this.DataContext = this.book;
            this.CheckIfImageExist();

        }
        private async void  ViewEditBook(object sender, RoutedEventArgs e) 
        {
            Button btn = sender as Button;
            Book book = btn.CommandParameter as Book;
            //if (this.editBook == null)
            this.editBook = new EditBook(book);

            this.editBook.Owner = this;
            bool? reponse = this.editBook.ShowDialog() ;
            if (reponse == true)
            {
                //ApiClient<Book> client = new ApiClient<Book>();
                //client.Scheme = "http";
                //client.Host = "localhost";
                //client.Port = 5049;
                //client.Path = "api/Guest/GetBook";
                //client.AddParameter("bookID", book.BookID);
                //this.book = await client.GetAsync();
                this.DataContext = null;
                this.DataContext = this.book;
                this.CheckIfImageExist();
            }

        }

        private void CheckIfImageExist()
        {
            if (this.book.BookImagePath.ToLower() != "none" && this.book.BookImagePath != null)
            {
                BookCoverImage.Visibility = Visibility.Visible;
                BookCoverEmoji.Visibility = Visibility.Collapsed;
            }
            else
            {
                BookCoverImage.Visibility = Visibility.Collapsed;
                BookCoverEmoji.Visibility = Visibility.Visible;
            }
        }
    }
}
