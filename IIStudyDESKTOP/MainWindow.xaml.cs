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
using IIStudyDESKTOP.UserControllers;
using IIStudyDESKTOP.WindowsPages;
namespace IIStudyDESKTOP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewBooks viewBooks;
        ViewBooksCatalog viewBooksCatalog;
        BookView bookView;
        ViewOrders viewOrders;
        RegisteredsPage registeredsPage;
        BookDetails bookDetails;
        ViewExams viewExams;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        

        

        

        private void ViewBooks(object sender, RoutedEventArgs e)
        {
            if (this.viewBooksCatalog == null)
                this.viewBooksCatalog = new ViewBooksCatalog();
            this.MainContent.Content = this.viewBooksCatalog;
            this.ClickButtonNav(sender);

        }

        private void ViewExams(object sender, RoutedEventArgs e)
        {
            if (this.viewExams == null)
                this.viewExams = new ViewExams();
            this.MainContent.Content = this.viewExams;
            this.ClickButtonNav(sender);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (this.bookView == null)
                this.bookView = new BookView();
            this.MainContent.Content = this.bookView;
        }
        private void ViewOrders(object sender, RoutedEventArgs e)
        {
            if (this.viewOrders == null)
                this.viewOrders = new ViewOrders();
            this.MainContent.Content = this.viewOrders;
            this.ClickButtonNav(sender);

        }

        private void ViewRegistereds(object sender, RoutedEventArgs e)
        {
            if (this.registeredsPage == null)
                this.registeredsPage = new RegisteredsPage();
            this.MainContent.Content = this.registeredsPage;
            this.ClickButtonNav(sender);
        }

        private void ClickButtonNav(object sender)
        {
            this.HomeNav.Style = (Style)this.Resources["SidebarButton"];
            this.BooksNav.Style = (Style)this.Resources["SidebarButton"];
            this.ExamsNav.Style = (Style)this.Resources["SidebarButton"];
            this.OrdersNav.Style = (Style)this.Resources["SidebarButton"];
            this.UsersNav.Style = (Style)this.Resources["SidebarButton"];
            this.ReportsNav.Style = (Style)this.Resources["SidebarButton"];
            this.SettingsNav.Style = (Style)this.Resources["SidebarButton"];

            Button btn = sender as Button;
            btn.Style = (Style)this.Resources["ActiveSidebarButton"];
        }
    }
}