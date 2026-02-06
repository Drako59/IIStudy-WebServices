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
using IIStudyDESKTOP.UserControl;
namespace IIStudyDESKTOP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
         HomePage homePage;
        LoginPage loginPage;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ViewStartPage()
        {
            if (this.homePage == null)
                this.homePage = new HomePage();
            this.Content = this.homePage;
        }

        private void ViewLoginPage()
        {
            if (this.loginPage == null)
                this.loginPage = new LoginPage();
            this.Content = this.loginPage;
        }
    }
}