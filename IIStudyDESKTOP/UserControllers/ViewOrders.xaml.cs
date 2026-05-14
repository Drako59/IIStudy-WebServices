using IIStudyDESKTOP.WindowsPages;
using IIstudyWSClient;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Reflection.Metadata.BlobBuilder;
namespace IIStudyDESKTOP.UserControllers
{
    /// <summary>
    /// Interaction logic for ViewOrders.xaml
    /// </summary>
    public partial class ViewOrders : UserControl
    {
        List<Order> orders;
        private OrderStatus FilterStatus { get; set; } = (OrderStatus)(-1);
        private string SearchText { get; set; } = "";
        private OrderBooksWindow orderBooksWindow { get; set; }

        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;

        public ViewOrders()
        {
            InitializeComponent();
            this.GetOrders();
            this.DataContext = this;

            //Loaded += (_, __) => LoadOrders();
        }



        private ObservableCollection<Order> _allOrders = new();
        private ObservableCollection<Order> _displayed = new();
        private string _filterMode = "All";
        private static readonly (string bg, string border)[] StatusColors =
        {
            ("#fffbeb", "#fbbf24"),  // 0 Pending
            ("#eef2ff", "#667eea"),  // 1 Processing
            ("#f0f9ff", "#0ea5e9"),  // 2 Shipped
            ("#f0fdf4", "#22c55e"),  // 3 Delivered
            ("#fef2f2", "#ef4444"),  // 4 Canceled
            ("#fdf4ff", "#a855f7"),  // 5 Refund
        };


        // ════════════════════════════════════════════════════════════
        //  LOAD DATA FROM DATABASE
        // ════════════════════════════════════════════════════════════
        private void LoadOrders()
        {
            EmptyState.Visibility = Visibility.Collapsed;
            this.Filter();
            UpdateStats();
        }

        // ════════════════════════════════════════════════════════════
        //  FILTER + SEARCH
        // ════════════════════════════════════════════════════════════
        

        private void UpdateStats()
        {
            this.TxtTotalOrders.Text = this.orders.Count().ToString();
            this.TxtDelivered.Text = this.orders.Where(o => { return (OrderStatus)o.DeliveryStatus == OrderStatus.Delivered; }).Count().ToString();
            this.TxtPending.Text = this.orders.Where(o => { return (OrderStatus)o.DeliveryStatus == OrderStatus.Pending; }).Count().ToString();
            this.TxtMoney.Text = "₪" + this.orders.Select(o => o.Total_price).ToList().Sum().ToString();
            EmptyState.Visibility = this.orders.Count == 0
                ? Visibility.Visible : Visibility.Collapsed;
        }

        // ════════════════════════════════════════════════════════════
        //  EVENT HANDLERS
        // ════════════════════════════════════════════════════════════
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.SearchText = this.SearchBox.Text;
            this.Filter();
        }

        

        private void SetActiveChip(Button active)
        {
            var inactive = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f1f5f9"));
            var inactiveFg = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#475569"));
            var activeBg = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#667eea"));

            foreach (var btn in new[] { FilterAll, FilterDelivered, FilterPending, FilterCanceled, FilterRefund,FilterShipped,FilterProcessing })
            {
                btn.Background = inactive;
                btn.Foreground = inactiveFg;
            }
            active.Background = activeBg;
            active.Foreground = new SolidColorBrush(Colors.White);
        }


        private async void GetOrders()
        {
            ApiClient<List<Order>> client = new ApiClient<List<Order>>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Admin/GetAllOrders";
            this.orders = await client.GetAsync();
            if (this.orders == null)
                this.orders = new List<Order>();
            this.OrdersListView.ItemsSource = this.orders;
            this.UpdateStats();

        }

        private void UpdateStatus(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Order order = btn.Tag as Order;
            UpdateDeliveryStatusWindow StatusWindow = new UpdateDeliveryStatusWindow(order);

            int statusCopy = order.DeliveryStatus;
            Window parentWindow = Window.GetWindow(this);
            StatusWindow.Owner = parentWindow;
            bool? result = StatusWindow.ShowDialog();
            if (result != true)
                order.DeliveryStatus = statusCopy;
            this.OrdersListView.ItemsSource = null;
            this.OrdersListView.ItemsSource = this.orders;
            this.UpdateStats();

        }
        private void ViewOrderDetails(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Order order = btn.Tag as Order;
            OrderDetails orderDetailsWindow = new OrderDetails(order);

            Window parentWindow = Window.GetWindow(this);
            orderDetailsWindow.Owner = parentWindow;
            orderDetailsWindow.Show();

            

        }
        private void ViewOrderBooks(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Order order = btn.Tag as Order;

            orderBooksWindow = new OrderBooksWindow(order);
            Window parentWindow = Window.GetWindow(this);
            orderBooksWindow.Owner = parentWindow;
            orderBooksWindow.Show();
        }

        private void FilterByChip(object sender, RoutedEventArgs e)
        {
            Button chip = sender as Button;
            this.FilterStatus = (OrderStatus)int.Parse(chip.Tag.ToString());
            SetActiveChip(chip);

            Filter();
        }

        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Filter();
        }

        private void Filter()
        {
            OrderStatus status = this.FilterStatus;

            
            List<Order> filtered;
            switch (status) {
                case OrderStatus.Pending:
                    filtered = this.orders.Where(b => (OrderStatus)b.DeliveryStatus == OrderStatus.Pending).ToList();
                    break;
                case OrderStatus.Processing:
                    filtered = this.orders.Where(b => (OrderStatus)b.DeliveryStatus == OrderStatus.Processing).ToList();
                    break;
                case OrderStatus.Shipped:
                    filtered = this.orders.Where(b => (OrderStatus)b.DeliveryStatus == OrderStatus.Shipped).ToList();
                    break;
                case OrderStatus.Delivered:
                    filtered = this.orders.Where(b => (OrderStatus)b.DeliveryStatus == OrderStatus.Delivered).ToList();
                    break;
                case OrderStatus.Canceled:
                    filtered = this.orders.Where(b => (OrderStatus)b.DeliveryStatus == OrderStatus.Canceled).ToList();
                    break;
                case OrderStatus.Refund:
                    filtered = this.orders.Where(b => (OrderStatus)b.DeliveryStatus == OrderStatus.Refund).ToList();
                    break;
                default:
                    filtered = this.orders;
                    break;
            }

            

            filtered = filtered.Where(o =>
            {
                const string format = "yyyy-MM-dd";

                DateTime orderDate = DateTime.ParseExact(
                                        o.Date,
                                        format,
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None
                                    );
                bool date = true;
                if (this.FromDate != null && this.ToDate != null)
                    date = orderDate >= this.FromDate.Value.Date && orderDate <= this.ToDate.Value.Date;
                else if (this.FromDate != null)
                    date = orderDate >= this.FromDate.Value.Date;
                else if (this.ToDate != null)
                    date = orderDate <= this.ToDate.Value.Date;

                bool passSearch = string.IsNullOrEmpty(this.SearchText)
                    || o.OrderID.ToString().Contains(this.SearchText.ToLower())
                    || o.Location.ToLower().Contains(this.SearchText.ToLower()); //|| o.Books.Any(b => b.Title.ToLower().Contains(search)

                return passSearch && date;
            }).ToList();

            _displayed = new ObservableCollection<Order>(filtered);
            OrdersListView.ItemsSource = _displayed;

            EmptyState.Visibility = filtered.Count == 0
                ? Visibility.Visible : Visibility.Collapsed;
            this.OrdersListView.ItemsSource =  filtered;


        }


//private void ApplyFilters()
        //{
        //    var search = SearchBox?.Text?.Trim().ToLower() ?? "";

        //    var filtered = this.orders.Where(o =>
        //    {
        //        bool passFilter = _filterMode switch
        //        {
        //            "Delivered" => o.DeliveryStatus == (int)OrderStatus.Delivered,
        //            "Pending" => o.DeliveryStatus == (int)OrderStatus.Pending,
        //            _ => true
        //        };

        //        bool passSearch = string.IsNullOrEmpty(search)
        //            || o.OrderID.ToString().Contains(search)
        //            || o.Location.ToLower().Contains(search); //|| o.Books.Any(b => b.Title.ToLower().Contains(search)

        //    return passFilter && passSearch;
        //    }).ToList();

        //    _displayed = new ObservableCollection<Order>(filtered);
        //    OrdersListView.ItemsSource = _displayed;

        //    EmptyState.Visibility = filtered.Count == 0
        //        ? Visibility.Visible : Visibility.Collapsed;
        //}
    }
}
