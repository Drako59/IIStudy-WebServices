using IIStudyDESKTOP.Pages;
using IIStudyDESKTOP.UserControllers;
using IIStudyDESKTOP.WindowsPages;
using IIstudyWSClient;
using LLStudy_Models.Models;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace IIStudyDESKTOP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewBooksCatalog viewBooksCatalog;
        BookView bookView;
        ViewOrders viewOrders;
        RegisteredsPage registeredsPage;
        ViewExams viewExams;
        ViewEvents viewEvents;
        private Registered RegisteredDetails { get; set; }
        private readonly string registeredID;
        public MainWindow(string registeredID)
        {
            InitializeComponent();
            this.registeredID = registeredID;
            this.Init_Page();
        }

        private async void Init_Page()
        {
            await this.LoadUserDetails();
        }

        private async Task LoadUserDetails()
        {
            try
            {
                ApiClient<Registered> client = new ApiClient<Registered>();
                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5049;
                client.Path = "api/Registered/profile";
                client.AddParameter("registeredID", this.registeredID);

                this.RegisteredDetails = await client.GetAsync();

                if (this.RegisteredDetails == null)
                {
                    MessageBox.Show(
                                "The operation failed.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                }

                this.DataContext = this.RegisteredDetails;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                                "Couldn't send the request due to network error on the host or the client.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
        

        

        

        private void ViewBooks(object sender, RoutedEventArgs e)
        {
            if (this.viewBooksCatalog == null)
                this.viewBooksCatalog = new ViewBooksCatalog();
            this.MainContent.Content = this.viewBooksCatalog;
            this.ClickButtonNav(sender);
            this.SetMainBackground2("#667eea", "#764ba2");


        }

        private void ViewExams(object sender, RoutedEventArgs e)
        {
            if (this.viewExams == null)
                this.viewExams = new ViewExams();
            this.MainContent.Content = this.viewExams;
            this.ClickButtonNav(sender);
            this.SetMainBackground2("#1976d2", "#1565c0");
            
        }

        private void ViewOrders(object sender, RoutedEventArgs e)
        {
            if (this.viewOrders == null)
                this.viewOrders = new ViewOrders();
            this.MainContent.Content = this.viewOrders;
            this.SetMainBackground2("#667eea", "#764ba2");
            this.ClickButtonNav(sender);

        }

        private void ViewRegistereds(object sender, RoutedEventArgs e)
        {
            if (this.registeredsPage == null)
                this.registeredsPage = new RegisteredsPage();
            this.MainContent.Content = this.registeredsPage;
            this.SetMainBackground2("#667eea", "#764ba2");
            this.ClickButtonNav(sender);
        }

        private void ViewEvents(object sender, RoutedEventArgs e)
        {
            
            if (this.viewEvents == null)
                this.viewEvents = new ViewEvents();
            this.MainContent.Content = this.viewEvents;
            this.SetMainBackground2("#14b8a6", "#0d9488");
            this.ClickButtonNav(sender);
        }

        private void ClickButtonNav(object sender)
        {
            this.HomeNav.Style = (Style)this.Resources["SidebarButton"];
            this.BooksNav.Style = (Style)this.Resources["SidebarButton"];
            this.ExamsNav.Style = (Style)this.Resources["SidebarButton"];
            this.OrdersNav.Style = (Style)this.Resources["SidebarButton"];
            this.UsersNav.Style = (Style)this.Resources["SidebarButton"];
            this.CalendarsNav.Style = (Style)this.Resources["SidebarButton"];
            //this.ReportsNav.Style = (Style)this.Resources["SidebarButton"];
            //this.SettingsNav.Style = (Style)this.Resources["SidebarButton"];

            Button btn = sender as Button;
            btn.Style = (Style)this.Resources["ActiveSidebarButton"];
        }

        private void SetMainBackground3(string color1, string color2, string color3)
        {
            LinearGradientBrush brush = new LinearGradientBrush();

            brush.StartPoint = new Point(0, 0);
            brush.EndPoint = new Point(1, 1);

            brush.GradientStops.Add(new GradientStop(
                (Color)ColorConverter.ConvertFromString(color1), 0.0));

            brush.GradientStops.Add(new GradientStop(
                (Color)ColorConverter.ConvertFromString(color2), 0.5));

            brush.GradientStops.Add(new GradientStop(
                (Color)ColorConverter.ConvertFromString(color3), 1.0));

            MainContentArea.Background = brush;
        }
        private void SetMainBackground2(string color1, string color2)
        {
            MainContentArea.Background = new LinearGradientBrush(
                (Color)ColorConverter.ConvertFromString(color1),
                (Color)ColorConverter.ConvertFromString(color2),
                new Point(0, 0),
                new Point(1, 1)
            );
        }


        private void SignOut(object sender, RoutedEventArgs e)
        {
            LogInPageWindow logInPageWindow = new LogInPageWindow();
            logInPageWindow.Show();
            this.Close();
        }
    }
}