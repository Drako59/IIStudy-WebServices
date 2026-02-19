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
namespace IIStudyDESKTOP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewBooks viewBooks;
        BookView bookView;
        ViewOrders viewOrders;
        RegisteredsPage registeredsPage;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        

        

        

        private void ViewBooks(object sender, RoutedEventArgs e)
        {
            if (this.viewBooks == null)
                this.viewBooks = new ViewBooks();
            this.MainContent.Content = this.viewBooks;
        }

        private void ViewBook(object sender, RoutedEventArgs e)
        {
            
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
        }

        private void ViewRegistereds(object sender, RoutedEventArgs e)
        {
            if (this.registeredsPage == null)
                this.registeredsPage = new RegisteredsPage();
            this.MainContent.Content = this.registeredsPage; ;
        }
    }
}