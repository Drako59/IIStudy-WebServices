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
using IIstudyWSClient;
using LLStudy_Models.Models;

namespace IIStudyDESKTOP.WindowsPages
{
    /// <summary>
    /// Interaction logic for UpdateDeliveryStatusWindow.xaml
    /// </summary>
    public partial class UpdateDeliveryStatusWindow : Window
    {
        private Order order { get; set; }


        public OrderStatus? SelectedStatus { get; private set; } = null;

        private int _orderId;

        // The currently highlighted card border
        private Border _selectedCard = null;

        // Accent colours per status (border + background when selected)
        private static readonly (string bg, string border)[] StatusColors =
        {
            ("#fffbeb", "#fbbf24"),  // 0 Pending
            ("#eef2ff", "#667eea"),  // 1 Processing
            ("#f0f9ff", "#0ea5e9"),  // 2 Shipped
            ("#f0fdf4", "#22c55e"),  // 3 Delivered
            ("#fef2f2", "#ef4444"),  // 4 Canceled
            ("#fdf4ff", "#a855f7"),  // 5 Refund
        };
        public UpdateDeliveryStatusWindow(Order order)
        {
            if (order == null)
            {
                MessageBox.Show("Couldn't load order.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            this.order = order;
            InitializeComponent();
            this.PreSelectCard(order.DeliveryStatus);
            this.TxtOrderID.Text = $"Order #{this.order.OrderID}";
        }

        // ────────────────────────────────────────────────────────────
        //  Card click — highlight selection
        // ────────────────────────────────────────────────────────────
        private void StatusCard_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Border card) return;
            SelectCard(card);
        }

        private void SelectCard(Border card)
        {
            // Reset previously selected card
            if (_selectedCard != null)
                _selectedCard.BorderBrush = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#e2e8f0"));

            _selectedCard = card;

            // Highlight with status-specific colour
            int idx = int.Parse(card.Tag.ToString());
            card.BorderBrush = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString(StatusColors[idx].border));
            card.Background = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString(StatusColors[idx].bg));

            this.order.DeliveryStatus = idx;
        }

        private void PreSelectCard(int statusInt)
        {
            Border card = statusInt switch
            {
                0 => Card0,
                1 => Card1,
                2 => Card2,
                3 => Card3,
                4 => Card4,
                5 => Card5,
                _ => null
            };
            if (card != null) SelectCard(card);
        }

        // ────────────────────────────────────────────────────────────
        //  Confirm — call web service and close
        // ────────────────────────────────────────────────────────────
        private async void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCard == null)
            {
                MessageBox.Show("Please select a status first.", "No Status Selected",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            try
            {
                // ── Call your web service ──────────────────────────
                // Adjust scheme / host / port / path as needed
                ApiClient<Order> client = new ApiClient<Order>();
                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5049;
                client.Path = "api/Admin/UpdateOrder";

                
                ApiResultModel<bool> result = await client.PostAsyncRet<Order, bool>(this.order);

                if (result == null || !result.Success || !result.Data)
                {
                    MessageBox.Show("Failed to update status. Please try again.",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Network error:\n{ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
