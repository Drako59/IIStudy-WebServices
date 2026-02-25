using IIstudyWSClient;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using LLStudy_Models.ViewModels.Guest;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
namespace IIStudyDESKTOP.WindowsPages
{
    /// <summary>
    /// Interaction logic for ViewReviews.xaml
    /// </summary>
    public partial class ViewReviews : Window
    {

        List<ViewReview> reviews;
        List<ViewReview> filtered;
        public ViewReviews(Book book)
        {
            InitializeComponent();
            MouseLeftButtonDown += (_, e) => { try { DragMove(); } catch { } };

            TxtBookTitle.Text = string.IsNullOrWhiteSpace(book.Book_name)
                ? $"Book #{book.BookID}" : book.Book_name;

            LoadReviews(book);
        }

        // ════════════════════════════════════════════════════════════
        //  LOAD REVIEWS
        // ════════════════════════════════════════════════════════════
        private async void LoadReviews(Book book)
        {
            ApiClient<List<ViewReview>> client = new ApiClient<List<ViewReview>>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/GetBookReviews";
            client.AddParameter("bookID", book.BookID);
            this.reviews = await client.GetAsync();
            this.filtered = this.reviews;
            this.DataContext = this.reviews;
            this.ReviewsList.ItemsSource = this.reviews;

        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();
    }
}

