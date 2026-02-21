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

namespace IIStudyDESKTOP.UserControllers
{
    /// <summary>
    /// Interaction logic for ViewBooks.xaml
    /// </summary>
    public partial class ViewBooks : UserControl
    {
        private ObservableCollection<Book> allBooks;
        private ObservableCollection<Book> filteredBooks;
        private List<Book> books;
        private BookDetails bookDetails;
        public ViewBooks()
        {
            InitializeComponent();
            this.LoadBooks();
        }

        private async Task GetBooks()
        {
            ApiClient<List<Book>> client = new ApiClient<List<Book>> ();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/GetBooks";
            this.books = await client.GetAsync();
            
            //UpdateStatistics();

        }
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
                allBooks = new ObservableCollection<Book>
                {
                    new Book
                    {
                        BookID = "BK-001",
                        Book_name = "To Kill a Mockingbird",
                        Author_name = "Harper Lee",
                        Book_price = 24.99,
                        In_stock = true,
                        SubjectID = "1"
                    },
                    new Book
                    {
                        BookID = "BK-001",
                        Book_name = "To Kill a Mockingbird",
                        Author_name = "Harper Lee",
                        Book_price = 24.99,
                        In_stock = true,
                        SubjectID = "1"
                    },
                    new Book
                    {
                        BookID = "BK-001",
                        Book_name = "To Kill a Mockingbird",
                        Author_name = "Harper Lee",
                        Book_price = 24.99,
                        In_stock = true,
                        SubjectID = "1"
                    },
                    new Book
                    {
                        BookID = "BK-001",
                        Book_name = "To Kill a Mockingbird",
                        Author_name = "Harper Lee",
                        Book_price = 24.99,
                        In_stock = true,
                        SubjectID = "1"
                    },
                    new Book
                    {
                        BookID = "BK-001",
                        Book_name = "To Kill a Mockingbird",
                        Author_name = "Harper Lee",
                        Book_price = 24.99,
                        In_stock = true,
                        SubjectID = "1"
                    },
                    new Book
                    {
                        BookID = "BK-002",
                        Book_name = "To Kill a Mockingbird",
                        Author_name = "Harper Lee",
                        Book_price = 24.99,
                        In_stock = true,
                        SubjectID = "1"
                    },
                    new Book
                    {
                        BookID = "BK-002",
                        Book_name = "To Kill a Mockingbird",
                        Author_name = "Harper Lee",
                        Book_price = 24.99,
                        In_stock = true,
                        SubjectID = "1"
                    } };

                //filteredBooks = new ObservableCollection<Book>(allBooks);
                dgBooks.ItemsSource = allBooks;

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
        private void UpdateStatistics()
        {
            txtTotalBooks.Text = this.books.Count.ToString();
            txtInStock.Text = this.books.Count(b => b.In_stock).ToString();
            txtOutOfStock.Text = this.books.Count(b => !b.In_stock).ToString();
        }

        private void ViewBookDetails(object sender, RoutedEventArgs e)
        {
            if (this.bookDetails == null)
                this.bookDetails = new BookDetails();
            else
            {
                this.bookDetails.Close();
                this.bookDetails = new BookDetails();
            }
                Window parentWindow = Window.GetWindow(this);
            this.bookDetails.Owner = parentWindow;
            this.bookDetails.Show();
        }
    }
}
