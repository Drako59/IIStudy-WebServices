using IIStudyDESKTOP.WindowsPages;
using IIstudyWSClient;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using LLStudy_Models.ViewModels.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
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
        private BookShownDesktop book;
        private Dictionary<string,string> Subjects { get; set; }
        public BookDetails(BookShownDesktop book, Dictionary<string, string> subjects)
        {
            this.Subjects = subjects;
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
            BookShownDesktop book = btn.CommandParameter as BookShownDesktop;
            //if (this.editBook == null)
            var json = JsonSerializer.Serialize(this.book);
            var copy = JsonSerializer.Deserialize<BookShownDesktop>(json);

            this.editBook = new EditBook(book, this.Subjects);

            bool? reponse = this.editBook.ShowDialog() ;
            
            if (reponse == true)
            {
                
                this.DataContext = null;
                this.DataContext = this.book;
                this.CheckIfImageExist();
            }
            else
            {
                //DOING THIS SO THE DATA WILL BE UPDATE ON ALL THE PAGES AND NOT ONLY THIS PAGE.
                this.book.Author_name = copy.Author_name;
                this.book.Book_name = copy.Book_name;
                this.book.Pdf_url_book = copy.Pdf_url_book;
                this.book.BookDetails = copy.BookDetails;
                this.book.Subject_name = copy.Subject_name;
                this.book.Book_price = copy.Book_price;
                this.book.IsOnline = copy.IsOnline;
                this.book.In_stock = copy.In_stock;
                this.book.BookImagePath = copy.BookImagePath;
                this.DataContext = null;
                this.DataContext = this.book;
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
