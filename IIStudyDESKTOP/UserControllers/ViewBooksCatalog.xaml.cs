using IIStudyDESKTOP.WindowsPages;
using IIstudyWSClient;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels.Guest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IIStudyDESKTOP.UserControllers
{
    /// <summary>
    /// Interaction logic for ViewBooksCatalog.xaml
    /// </summary>
    public partial class ViewBooksCatalog : UserControl
    {
        private List<BookShownDesktop> filteredBooks;
        private List<BookShownDesktop> books;
        private BookDetails bookDetails;
        private ViewReviews reviews;
        private CreateBookPage createBookPage;
        private List<string> SubjectsNames { get; set; }
        private string SelectedSubject { get; set; }
        private Dictionary<string, string> Subjects { get; set; }
        private List<SubjectDetails> SubjectsDetails { get; set; }
        private SubjectsWindow SubjectsWindow { get; set; }
        private string searchBar;

        public ViewBooksCatalog()
        {
            InitializeComponent();
            this.init_page();

        }

        private async Task init_page()
        {
            await this.LoadSubjectsDict();
            await this.LoadSubjectsDetails();
            await this.LoadBooks();
        }

        private async Task GetBooks()
        {
            ApiClient<List<BookShownDesktop>> client = new ApiClient<List<BookShownDesktop>>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/GetDesktopBooks";
            this.books = await client.GetAsync();
            this.filteredBooks = this.books;
            //UpdateStatistics();

        }
        private async Task LoadSubjectsDict()
        {
            ApiClient<Dictionary<string, string>> client = new ApiClient<Dictionary<string, string>>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/GetAllSubjectsDict";
            this.Subjects = await client.GetAsync();

            if(this.Subjects == null)
            {
                MessageBox.Show("Faild to Load subjects", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Subjects = new Dictionary<string, string>();
            }
            this.SubjectsNames = this.Subjects.Values.ToList();
            this.Subjects.Add("0", "All");
           
            //this.SubjectsList.ItemsSource = this.Subjects;
            this.TxtSubjectBadge.Text = this.Subjects.Count().ToString();
            this.SubjectsNames.Insert(0, "All");


           

        }

        private async Task LoadSubjectsDetails(){
            ApiClient<List<SubjectDetails>> client = new ApiClient<List<SubjectDetails>>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/GetAllSubjectsDetails";
            this.SubjectsDetails = await client.GetAsync();
            if (this.SubjectsDetails == null)
            {
                MessageBox.Show("Faild to Load subjects", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.SubjectsDetails = new List<SubjectDetails>();
            }

            this.SubjectsDetails.Insert(0,new SubjectDetails() { BooksCount = 0, SubjectID = "0", Subject_name = "All"});

            this.SubjectsList.ItemsSource = this.SubjectsDetails;

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
            this.BooksList.ItemsSource = this.books;
            this.UpdateStatistics();
        }
        

        

        private void SearchUpdate(object sender, TextChangedEventArgs e)
        {
            this.filteredBooks = this.books;
            this.searchBar = this.SearchBox.Text;
            ApplyFilters();
            this.UpdateStatistics();
        }


        private void InStockFillter(object sender, MouseButtonEventArgs e)
        {
            this.filteredBooks = this.books.Where(b => b.In_stock).ToList();
            this.BooksList.ItemsSource = this.filteredBooks;
            SetActiveChip(ChipInStock);
            this.ApplyFilters();
        }

        private void OutStockFillter(object sender, MouseButtonEventArgs e)
        {
            this.filteredBooks = this.books.Where(b => !b.In_stock).ToList();
            this.BooksList.ItemsSource = this.filteredBooks;
            SetActiveChip(ChipOutOfStock);
            this.ApplyFilters();
        }

        private void ActiveFillter(object sender, MouseButtonEventArgs e)
        {
            this.filteredBooks = this.books.Where(b => !b.IsDeleted).ToList();
            this.BooksList.ItemsSource = this.filteredBooks;
            SetActiveChip(ChipActive);
            this.ApplyFilters();
        }
        private void DeletedFillter(object sender, MouseButtonEventArgs e)
        {
            this.filteredBooks = this.books.Where(b => b.IsDeleted).ToList();
            this.BooksList.ItemsSource = this.filteredBooks;
            SetActiveChip(ChipDeleted);
            this.ApplyFilters();
        }
        private void AllFillter(object sender, MouseButtonEventArgs e)
        {
            
            this.BooksList.ItemsSource = this.books;
            this.filteredBooks = this.books;
            SetActiveChip(ChipAll);
            this.ApplyFilters();
        }

        private void SubjectFilter(object sender, MouseButtonEventArgs e)
        {
            Border btn = sender as Border;
            string key = btn.Tag as string;

            this.filteredBooks = this.books.Where(b => { return b.SubjectID == key || key == "0"; }).ToList();
            this.ApplyFilters();

        }

        private void SetActiveChip(Border active)
        {
            // RESET
            ChipAll.Background = new SolidColorBrush(Colors.Transparent);
            ChipInStock.Background = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#f0fdf4"));
            ChipOutOfStock.Background = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#fef2f2"));
            ChipActive.Background = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#f0fdf4"));
            ChipDeleted.Background = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#fef2f2"));
            ChipAllText.Foreground = Brushes.Black;
            ChipInStockText.Foreground = Brushes.Green;
            ChipOutStockText.Foreground = Brushes.Red;
            ChipActiveText.Foreground = Brushes.Green;
            ChipDeletedText.Foreground = Brushes.Red;

            // ACTIVE STATES
            if (active == ChipAll)
            {
                active.Background = new LinearGradientBrush(
                    (Color)ColorConverter.ConvertFromString("#1565c0"),
                    (Color)ColorConverter.ConvertFromString("#7c6fd4"),
                    new Point(0, 0.5), new Point(1, 0.5));
            }

            else if (active == ChipInStock || active == ChipActive)
            {
                active.Background = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#16a34a"));
            }

            else if (active == ChipOutOfStock || active == ChipDeleted)
            {
                active.Background = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#dc2626"));
            }
        }

        private void ApplyFilters()
        {
            this.searchBar = this.searchBar != null ? this.searchBar.Trim().ToLower() : "";
            //if (this.searchBar == null) this.SearchPlaceholder.Text = "Search books.....";
            //else
            //{
            //    this.SearchPlaceholder.Text = "";
            //}
            this.filteredBooks = this.filteredBooks.Where(b =>
            {
                bool search = b.Book_name.ToLower().Contains(this.searchBar) ||
                                b.Author_name.ToLower().Contains(this.searchBar) ||
                                b.BookID.ToLower() == this.searchBar;


                return search;
            }
            ).ToList();

            //this.DataContext = this.filteredBooks;
            this.BooksList.ItemsSource = this.filteredBooks;

        }
        private void UpdateStatistics()
        {
            if (this.filteredBooks != null && this.filteredBooks.Any())
            {
                TxtTotalBooks.Text = this.filteredBooks.Count.ToString();
                TxtInStock.Text = this.filteredBooks.Count(b => b.In_stock).ToString();
                TxtOutOfStock.Text = this.filteredBooks.Count(b => !b.In_stock).ToString();
                TxtSubjectCount.Text = this.Subjects.Count.ToString();
            }
        }

        private void ViewBookDetails(object sender, RoutedEventArgs e)
        {

            Button btn = sender as Button;
            BookShownDesktop book = btn.Tag as BookShownDesktop;
            if (this.bookDetails == null)
                this.bookDetails = new BookDetails(book, this.Subjects);
            else
            {
                //this.bookDetails.Close();
                this.bookDetails = new BookDetails(book, this.Subjects);
            }
            Window parentWindow = Window.GetWindow(this);
            this.bookDetails.Owner = parentWindow;
            this.bookDetails.Show();
            this.BooksList.ItemsSource = null;
            this.BooksList.ItemsSource = this.books;
        }

        private async void DeleteBook(object sender, RoutedEventArgs e)
        {

            Button btn = sender as Button;
            Book book = btn.Tag as Book;
            ApiClient<string> client = new ApiClient<string>();
            ApiResultModel<bool> result = new ApiResultModel<bool>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Admin/RemoveBook";
            result = await client.PostAsyncRet<Book, bool>(book);
            if (!result.Success || !result.Data)
            {
                MessageBox.Show("Delete was failed.", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                book.IsDeleted = true;
                //this.filteredBooks = this.books;
                //this.books.Remove((BookShownDesktop)book);
                this.DataContext = null;
                this.BooksList.ItemsSource = null;
                this.DataContext = this.books;
                this.BooksList.ItemsSource = this.books;
                //MessageBox.Show("Delete succeed", "Validation", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private async void RestoreBook(object sender, RoutedEventArgs e)
        {

            Button btn = sender as Button;
            Book book = btn.Tag as Book;
            ApiClient<string> client = new ApiClient<string>();
            ApiResultModel<bool> result = new ApiResultModel<bool>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Admin/RestoreBook";
            result = await client.PostAsyncRet<Book, bool>(book);
            if (!result.Success || !result.Data)
            {
                MessageBox.Show("Restore was failed.", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                book.IsDeleted = false;
                //this.filteredBooks = this.books;
                //this.books.Remove((BookShownDesktop)book);
                this.DataContext = null;
                this.BooksList.ItemsSource = null;
                this.DataContext = this.books;
                this.BooksList.ItemsSource = this.books;
                //MessageBox.Show("Restore succeed.", "Validation", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void ToggleDeleteButton(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Book book = btn.Tag as Book;

            if (!book.IsDeleted)
            {
                var confirm = MessageBox.Show(
                $"Are you sure you want to remove book \"{book.Book_name}\"?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

                if (confirm != MessageBoxResult.Yes) return;
                this.DeleteBook(sender, e);
            }
            else
            {
                var confirm = MessageBox.Show(
                $"Are you sure you want to Restore book \"{book.Book_name}\"?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

                if (confirm != MessageBoxResult.Yes) return;
                this.RestoreBook(sender, e);
            }
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
                this.reviews = new ViewReviews(book, reviewsAmount: details.reviewsNum, avgRate: details.Rate);
            else
            {
                this.reviews = new ViewReviews(book, reviewsAmount: details.reviewsNum, avgRate: details.Rate);
            }
            Window parentWindow = Window.GetWindow(this);

            this.reviews.Owner = parentWindow;
            this.reviews.Show();

        }
        private void ViewSubjects(object sender, RoutedEventArgs e)
        {
            this.SubjectsWindow = new SubjectsWindow();
            Window parentWindow = Window.GetWindow(this);
            this.SubjectsWindow.Owner = parentWindow;
            this.SubjectsWindow.Show();
        }
        private void CreateBookPopUp(object sender, RoutedEventArgs e)
        {

            this.createBookPage = new CreateBookPage(this.Subjects);

            this.createBookPage.ShowDialog();
        }

        private void FilterSubject(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string subjectID = comboBox.SelectedValue?.ToString();
            if (subjectID != "0")
            {
                this.filteredBooks = this.books.Where(b =>
                {
                    bool found = (b.SubjectID == subjectID);
                    return found;

                }
                ).ToList();
                this.BooksList.ItemsSource = null;
                this.BooksList.ItemsSource = this.filteredBooks;
                //this.DataContext = this.filteredBooks;
            }
            else
            {
                this.BooksList.ItemsSource = null;
                this.BooksList.ItemsSource = this.books;
                //this.DataContext = this.books;
            }

        }

        private void SearchSubject(object sender, RoutedEventArgs e)
        {
            string searchText = this.SubjectSearchBox.Text;
            this.SubjectsList.ItemsSource = this.SubjectsDetails.Where(s => s.Subject_name.ToLower().Contains(searchText.ToLower())).ToList();
        }

        
    }
}
