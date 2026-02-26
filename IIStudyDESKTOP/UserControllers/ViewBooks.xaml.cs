using IIStudyDESKTOP.WindowsPages;
using IIstudyWSClient;
using LLStudy_Models;
using LLStudy_Models.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IIStudyDESKTOP.WindowsPages;
using System.Runtime.CompilerServices;
using LLStudy_Models.ViewModels;
using LLStudy_Models.ViewModels.Guest;

namespace IIStudyDESKTOP.UserControllers
{
    /// <summary>
    /// Interaction logic for ViewBooks.xaml
    /// </summary>
    public partial class ViewBooks : UserControl
    {
        //private ObservableCollection<Book> allBooks;
        //private List<ViewBookViewModel> fullBooks;
        private List<BookShownDesktop> filteredBooks;
        private List<BookShownDesktop> books;
        private BookDetails bookDetails;
        private ViewReviews reviews;
        private CreateBookPage createBookPage;
        private string searchBar;
        
        public ViewBooks()
        {
            InitializeComponent();
            this.LoadBooks();
        }

        private async Task GetBooks()
        {
            ApiClient<List<BookShownDesktop>> client = new ApiClient<List<BookShownDesktop>> ();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/GetDesktopBooks";
            this.books = await client.GetAsync();
            this.filteredBooks = this.books;
            //UpdateStatistics();

        }

        //private async Task GetFullBooks()
        //{
        //    ApiClient<List<Book>> client = new ApiClient<List<Book>>();
        //    client.Scheme = "http";
        //    client.Host = "localhost";
        //    client.Port = 5049;
        //    client.Path = "api/Guest/GetBooks";
        //    this.books = await client.GetAsync();
        //    this.filteredBooks = this.books;
        //    //UpdateStatistics();

        //}
        private async Task LoadBooks()
        {
            await this.GetBooks();
            this.DataContext = this.books;
            this.dgBooks.ItemsSource = this.books;
            this.UpdateStatistics();
        }
        private async Task LoadBooks(int removeThisAfter)
        {
            try
            {
                // Load books from your database
                // For demo purposes, using sample data
                //allBooks = new ObservableCollection<Book>
                //{
                //    new Book
                //    {
                //        BookID = "BK-001",
                //        Book_name = "To Kill a Mockingbird",
                //        Author_name = "Harper Lee",
                //        Book_price = 24.99,
                //        In_stock = true,
                //        SubjectID = "1"
                //    },
                //    new Book
                //    {
                //        BookID = "BK-001",
                //        Book_name = "To Kill a Mockingbird",
                //        Author_name = "Harper Lee",
                //        Book_price = 24.99,
                //        In_stock = true,
                //        SubjectID = "1"
                //    },
                //    new Book
                //    {
                //        BookID = "BK-001",
                //        Book_name = "To Kill a Mockingbird",
                //        Author_name = "Harper Lee",
                //        Book_price = 24.99,
                //        In_stock = true,
                //        SubjectID = "1"
                //    },
                //    new Book
                //    {
                //        BookID = "BK-001",
                //        Book_name = "To Kill a Mockingbird",
                //        Author_name = "Harper Lee",
                //        Book_price = 24.99,
                //        In_stock = true,
                //        SubjectID = "1"
                //    },
                //    new Book
                //    {
                //        BookID = "BK-001",
                //        Book_name = "To Kill a Mockingbird",
                //        Author_name = "Harper Lee",
                //        Book_price = 24.99,
                //        In_stock = true,
                //        SubjectID = "1"
                //    },
                //    new Book
                //    {
                //        BookID = "BK-002",
                //        Book_name = "To Kill a Mockingbird",
                //        Author_name = "Harper Lee",
                //        Book_price = 24.99,
                //        In_stock = true,
                //        SubjectID = "1"
                //    },
                //    new Book
                //    {
                //        BookID = "BK-002",
                //        Book_name = "To Kill a Mockingbird",
                //        Author_name = "Harper Lee",
                //        Book_price = 24.99,
                //        In_stock = true,
                //        SubjectID = "1"
                //    } };

                ////filteredBooks = new ObservableCollection<Book>(allBooks);
                //dgBooks.ItemsSource = allBooks;

                UpdateStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error loading books: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void SearchUpdate(object sender, TextChangedEventArgs e)
        {
            this.searchBar = this.txtSearch.Text;
            ApplyFilters();
            this.UpdateStatistics();
        }
        private void ApplyFilters()
        {
            this.searchBar = this.searchBar != null ? this.searchBar.Trim().ToLower() : "";
            this.filteredBooks = this.books.Where(b =>
            {
                bool search = b.Book_name.ToLower().Contains(this.searchBar) || 
                              b.Author_name.ToLower().Contains(this.searchBar) ||
                              b.BookID.ToLower() == this.searchBar;


                return search;
            }
            ).ToList();
            //this.DataContext = this.filteredBooks;
            this.dgBooks.ItemsSource = this.filteredBooks;

        }
        private void UpdateStatistics()
        {
            txtTotalBooks.Text = this.filteredBooks.Count.ToString();
            txtInStock.Text = this.filteredBooks.Count(b => b.In_stock).ToString();
            txtOutOfStock.Text = this.filteredBooks.Count(b => !b.In_stock).ToString();
        }

        private void ViewBookDetails(object sender, RoutedEventArgs e)
        {

            Button btn = sender as Button;
            Book book = btn.Tag as Book;
            if (this.bookDetails == null)
                this.bookDetails = new BookDetails(book);
            else
            {
                //this.bookDetails.Close();
                this.bookDetails = new BookDetails(book);
            }
            Window parentWindow = Window.GetWindow(this);
            this.bookDetails.Owner = parentWindow;
            this.bookDetails.Show();
            this.dgBooks.ItemsSource = null;
            this.dgBooks.ItemsSource = this.books;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ViewReviewsPage(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Book book = btn.CommandParameter as Book;
            BookShownDesktop details = btn.CommandParameter as BookShownDesktop;
            if (this.reviews == null)
                this.reviews = new ViewReviews(book,reviewsAmount: details.reviewsNum, avgRate: details.Rate);
            else
            {
                this.reviews = new ViewReviews(book, reviewsAmount: details.reviewsNum, avgRate: details.Rate);
            }
            Window parentWindow = Window.GetWindow(this);

            this.reviews.Owner = parentWindow;
            this.reviews.Show();
            
        }

        private void CreateBookPopUp(object sender, RoutedEventArgs e)
        {

            this.createBookPage = new CreateBookPage();

            this.createBookPage.ShowDialog();
        }
    }
}
