using IIStudyDESKTOP.Pages;
using System.Configuration;
using System.Data;
using System.Windows;

namespace IIStudyDESKTOP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            LogInPageWindow logInPageWindow = new LogInPageWindow();
            logInPageWindow.Show();
        }
    }

}
