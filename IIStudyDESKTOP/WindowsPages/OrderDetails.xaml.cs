using System;
using System.Collections.Generic;
using System.Linq;
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
using IIstudyWSClient;
using LLStudy_Models.Models;
namespace IIStudyDESKTOP.WindowsPages
{
    /// <summary>
    /// Interaction logic for OrderDetails.xaml
    /// </summary>
    public partial class OrderDetails : Window
    {
        private Order Order { get; set; }
        private List<OrderBook> OrderBooks { get; set; }
        public OrderDetails(Order order)
        {
            this.Order = order;
            this.DataContext = this.Order;
            InitializeComponent();
            LoadOrderBooks();
        }

        private async void LoadOrderBooks()
        {
            try
            {
                ApiClient<List<OrderBook>> client = new ApiClient<List<OrderBook>>();
                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5049;
                client.Path = "api/Admin/GetOrderBooks";
                client.AddParameter("orderID", this.Order.OrderID);
                this.OrderBooks = await client.GetAsync();

                if (this.OrderBooks == null)
                {
                    this.OrderBooks = new List<OrderBook>();
                    MessageBox.Show("Failed in reciving the books from web service", "Request Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                this.BooksList.ItemsSource = this.OrderBooks;
                this.TxtBookCount.Text = this.OrderBooks.Count().ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Faild to connect to host.", "Network Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void Close_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
