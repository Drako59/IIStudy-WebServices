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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IIStudyDESKTOP.UserControllers
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();
        }
        private async void GoToBooks_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.ViewBooks(mainWindow.BooksNav, e);
            }
        }
        private async void GoToExams_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.ViewExams(mainWindow.ExamsNav, e);
            }
        }
        private async void GoToOrders_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.ViewOrders(mainWindow.OrdersNav, e);
            }
        }

        private async void GoToRegistereds_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.ViewRegistereds(mainWindow.UsersNav, e);
            }
        }
        private async void GoToCalendar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.ViewEvents(mainWindow.CalendarsNav, e);
            }
        }
    }
}
