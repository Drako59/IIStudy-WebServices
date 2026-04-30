using IIstudyWSClient;
using LLStudy_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
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
    
    public partial class OrderBooksWindow : Window
    {
        private List<OrderBook> OrderBooks { get; set; }
        private Order Order { get; set; }

        private int _activeFilter { get; set; }
        public OrderBooksWindow(Order order)
        {
            InitializeComponent();

            this.Order = order;
            this.TxtOrderTitle.Text = $"Order #{this.Order?.OrderID ?? ""}";
            this.TxtTotalValue.Text = $"₪{this.Order?.Total_price ?? 0 }";
            this._activeFilter = 9;
            LoadOrderBooks();
        }

        private async Task LoadOrderBooks()
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
                UpdateBooksCount();
                this.ApplyFilter();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Faild to connect to host.", "Network Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void UpdateBooksCount()
        {
            this.TxtBookCount.Text = this.OrderBooks.Count().ToString();
        }
        private void Chip_All(object sender, MouseButtonEventArgs e)
        {
            _activeFilter = 0;
            SetActiveChip(ChipAll);
            ApplyFilter();
        }

        private void Chip_Physical(object sender, MouseButtonEventArgs e)
        {
            _activeFilter = 1;
            SetActiveChip(ChipPhysical);
            ApplyFilter();
        }

        private void Chip_Online(object sender, MouseButtonEventArgs e)
        {
            _activeFilter = 2;
            SetActiveChip(ChipOnline);
            ApplyFilter();
        }

        private void SetActiveChip(Border active)
        {
            // Reset all to translucent white
            ChipAll.Background = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#cac8cc"));
            ChipPhysical.Background = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#cac8cc"));
            ChipOnline.Background = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#cac8cc"));

            // Active = solid white
            active.Background = new SolidColorBrush(Colors.White);

            // Fix text colour: active = purple, inactive = white
            //SetChipTextColor(ChipAll, active == ChipAll);
            //SetChipTextColor(ChipPhysical, active == ChipPhysical);
            //SetChipTextColor(ChipOnline, active == ChipOnline);
        }

        //private void SetChipTextColor(System.Windows.Controls.Border chip, bool isActive)
        //{
        //    // Walk the visual tree to find TextBlocks inside the chip
        //    foreach (var tb in FindTextBlocks(chip))
        //    {
        //        // Skip emoji TextBlocks (they contain emoji chars)
        //        if (tb.Text.Length <= 2) continue;
        //        tb.Foreground = isActive
        //            ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#667eea"))
        //            : new SolidColorBrush(Colors.White);
        //    }
        //}
        private void ApplyFilter()
        {
            var filtered = _activeFilter switch
            {
                1 => this.OrderBooks.Where(b => !b.IsOnline).ToList(),
                2 => this.OrderBooks.Where(b => b.IsOnline).ToList(),
                _ => this.OrderBooks
            };

            BooksList.ItemsSource = filtered;

            int count = filtered.Count();
            TxtBookCount.Text = $"{count} book{(count == 1 ? "" : "s")}";
            

            EmptyState.Visibility = count == 0
                ? Visibility.Visible : Visibility.Collapsed;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
