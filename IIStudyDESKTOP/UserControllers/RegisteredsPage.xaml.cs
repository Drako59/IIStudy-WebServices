using IIstudyWSClient;
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
using static System.Reflection.Metadata.BlobBuilder;

namespace IIStudyDESKTOP.UserControllers
{
    /// <summary>
    /// Interaction logic for UsersPage.xaml
    /// </summary>
    public partial class RegisteredsPage : UserControl
    {


        private List<Registered> registereds = new List<Registered>();

        private ObservableCollection<Registered> _allUsers = new();
        private string _filterMode = "All";

        public RegisteredsPage()
        {
            InitializeComponent();
            Loaded += (_, __) => LoadUsers();
        }

        // ════════════════════════════════════════════════════════════
        //  LOAD DATA
        // ════════════════════════════════════════════════════════════
        private async Task LoadUsers()
        {
            //_allUsers.Clear();
            if (this.registereds != null && this.registereds.Any())
            {
                ApplyFilters();
                this.registereds.Clear();
            }
            await GetRegistereds();
            UpdateStats();






        }

        private async Task GetRegistereds()
        {
            ApiClient<List<Registered>> client = new ApiClient<List<Registered>>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Admin/GetAllRegistereds";
            this.registereds.AddRange(await client.GetAsync());
            
            this.DataContext = this.registereds;
            this.UsersListView.ItemsSource = this.registereds;
        }

        // ════════════════════════════════════════════════════════════
        //  FILTER + SEARCH
        // ════════════════════════════════════════════════════════════
        private void ApplyFilters()
        {
            var search = SearchBox?.Text?.Trim().ToLower() ?? "";

            var filtered = this.registereds.Where(u =>
            {
                bool passFilter = _filterMode switch
                {
                    "Admin" => u.Role?.ToLower() == "admin",
                    "Manager" => u.Role?.ToLower() == "manager",
                    "User" => u.Role?.ToLower() == "user",
                    _ => true
                };

                bool passSearch = string.IsNullOrEmpty(search)
                    || u.UserName.ToLower().Contains(search)
                    || u.Email.ToLower().Contains(search)
                    || u.Role.ToLower().Contains(search)
                    || u.RegisteredID.ToString().Contains(search);

                return passFilter && passSearch;
            }).ToList();

            UsersListView.ItemsSource = new ObservableCollection<Registered>(filtered);

            EmptyState.Visibility = filtered.Count == 0
                ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateStats()
        {
            TxtTotal.Text = this.registereds.Count.ToString();
            TxtAdmins.Text = this.registereds.Count(u => u.Role?.ToLower() == "admin").ToString();
            TxtManagers.Text = this.registereds.Count(u => u.Role?.ToLower() == "manager").ToString();
            TxtUsers.Text = this.registereds.Count(u => u.Role?.ToLower() == "user").ToString();
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
        private void Filter_Admins(object sender, RoutedEventArgs e)
        {
            _filterMode = "Admin";
            SetActiveChip(FilterAdmins);
            ApplyFilters();
        }
        private void Filter_Managers(object sender, RoutedEventArgs e)
        {
            _filterMode = "Manager";
            SetActiveChip(FilterManagers);
            ApplyFilters();
        }
        private void Filter_Users(object sender, RoutedEventArgs e)
        {
            _filterMode = "User";
            SetActiveChip(FilterUsers);
            ApplyFilters();
        }

        private void SetActiveChip(Button active)
        {
            var inactiveBg = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f1f5f9"));
            var inactiveFg = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#475569"));
            var activeBg = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#667eea"));

            foreach (var btn in new[] { FilterAll, FilterAdmins, FilterManagers, FilterUsers })
            {
                btn.Background = inactiveBg;
                btn.Foreground = inactiveFg;
            }
            active.Background = activeBg;
            active.Foreground = new SolidColorBrush(Colors.White);
        }

        // ── View Info ────────────────────────────────────────────────
        private void ViewUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || btn.Tag is not string id) return;
            var user = _allUsers.FirstOrDefault(u => u.RegisteredID == id);
            if (user is null) return;

            MessageBox.Show(
                $"👤  User Info\n" +
                $"─────────────────────────\n" +
                $"ID       : {user.RegisteredID}\n" +
                $"Username : {user.UserName}\n" +
                $"Email    : {user.Email}\n" +
                $"Role     : {user.Role}\n" +
                $"Birth    : {user.Birth}\n" +
                //$"Phone    : {user.Phone}\n" +
                $"Image    : {user.ImagePath}",
                "User Details",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        // ── Delete User ──────────────────────────────────────────────
        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || btn.Tag is not string id) return;
            var user = _allUsers.FirstOrDefault(u => u.RegisteredID == id);
            if (user is null) return;

            var confirm = MessageBox.Show(
                $"Are you sure you want to remove user \"{user.UserName}\"?\nThis action cannot be undone.",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes) return;

            try
            {
                

                _allUsers.Remove(user);
                ApplyFilters();
                UpdateStats();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Delete failed:\n{ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
