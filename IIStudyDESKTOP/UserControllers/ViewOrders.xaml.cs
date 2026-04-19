using IIstudyWSClient;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
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
using static System.Reflection.Metadata.BlobBuilder;
namespace IIStudyDESKTOP.UserControllers
{
    /// <summary>
    /// Interaction logic for ViewOrders.xaml
    /// </summary>
    public partial class ViewOrders : UserControl
    {
        List<Order> orders;
        public ViewOrders()
        {
            InitializeComponent();
            this.GetOrders();
            //Loaded += (_, __) => LoadOrders();
        }

        private const string ConnStr =
            @"Server=YOUR_SERVER;Database=YOUR_DATABASE;Trusted_Connection=True;";

        private ObservableCollection<Order> _allOrders = new();
        private ObservableCollection<Order> _displayed = new();
        private string _filterMode = "All";

        

        // ════════════════════════════════════════════════════════════
        //  LOAD DATA FROM DATABASE
        // ════════════════════════════════════════════════════════════
        private void LoadOrders()
        {
            EmptyState.Visibility = Visibility.Collapsed;

            ApplyFilters();
            UpdateStats();
        }

        // ════════════════════════════════════════════════════════════
        //  FILTER + SEARCH
        // ════════════════════════════════════════════════════════════
        private void ApplyFilters()
        {
            var search = SearchBox?.Text?.Trim().ToLower() ?? "";

            var filtered = _allOrders.Where(o =>
            {
                bool passFilter = _filterMode switch
                {
                    "Delivered" => o.DeliveryStatus == (int)OrderStatus.Delivered,
                    "Pending" => o.DeliveryStatus == (int)OrderStatus.Pending,
                    _ => true
                };

                bool passSearch = string.IsNullOrEmpty(search)
                    || o.OrderID.ToString().Contains(search)
                    || o.Location.ToLower().Contains(search); //|| o.Books.Any(b => b.Title.ToLower().Contains(search)

            return passFilter && passSearch;
            }).ToList();

            _displayed = new ObservableCollection<Order>(filtered);
            OrdersListView.ItemsSource = _displayed;

            EmptyState.Visibility = filtered.Count == 0
                ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateStats()
        {
            TxtTotalOrders.Text = _allOrders.Count.ToString();
            TxtDelivered.Text = _allOrders.Count(o => o.DeliveryStatus == (int)OrderStatus.Delivered).ToString();
            TxtPending.Text = _allOrders.Count(o => o.DeliveryStatus == (int)OrderStatus.Pending).ToString();
            TxtRevenue.Text = $"₪{_allOrders.Sum(o => o.Total_price):N0}";
        }

        // ════════════════════════════════════════════════════════════
        //  EVENT HANDLERS
        // ════════════════════════════════════════════════════════════
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
            => ApplyFilters();

        private void Filter_All(object sender, RoutedEventArgs e)
        {
            _filterMode = "All";
            SetActiveChip(FilterAll);
            ApplyFilters();
        }

        private void Filter_Delivered(object sender, RoutedEventArgs e)
        {
            _filterMode = "Delivered";
            SetActiveChip(FilterDelivered);
            ApplyFilters();
        }

        private void Filter_Pending(object sender, RoutedEventArgs e)
        {
            _filterMode = "Pending";
            SetActiveChip(FilterPending);
            ApplyFilters();
        }

        private void SetActiveChip(Button active)
        {
            var inactive = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f1f5f9"));
            var inactiveFg = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#475569"));
            var activeBg = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#667eea"));

            foreach (var btn in new[] { FilterAll, FilterDelivered, FilterPending })
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
            this.DataContext = this.orders;
            this.OrdersListView.ItemsSource = this.orders;
        }
        

       
    }
}
