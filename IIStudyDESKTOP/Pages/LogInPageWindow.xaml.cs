using IIstudyWSClient;
using LLStudy_Models.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
using LLStudy_Models.ViewModels;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Windows.Media.TextFormatting;

namespace IIStudyDESKTOP.Pages
{
    /// <summary>
    /// Interaction logic for LogInPageWindow.xaml
    /// </summary>
    public partial class LogInPageWindow : Window
    {
        private SignInViewModel SignInViewModel { get; set; } = new SignInViewModel();
        private MainWindow MainWindow { get; set; }

        private bool isPasswordKeyDownEnabled = true;

        public LogInPageWindow()
        {
            InitializeComponent();
            this.DataContext = this.SignInViewModel;
            Loaded += (_, __) => TxtUsername.Focus();

        }

        private void Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && this.isPasswordKeyDownEnabled) this.LogInButton(sender, null);
        }

        private bool CheckValidation()
        {
            this.SignInViewModel.Validate();
            if (this.SignInViewModel.HasErrors)
            {
                MessageBox.Show(
                               "One or more field are not set with default value",
                               "Error",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void LogInButton(object sender , RoutedEventArgs e)
        {
            
            this.LogIn();
        }

        private async void LogIn()
        {
            this.SignInButton.IsEnabled = false;
            this.isPasswordKeyDownEnabled = false;
            this.SignInViewModel.Password = this.TxtPassword.Password;

            if (this.CheckValidation())
            {
                try
                {
                    ApiClient<bool> client = new ApiClient<bool>();
                    client.Scheme = "http";
                    client.Host = "localhost";
                    client.Port = 5049;
                    client.Path = "api/Guest/AdminSignIn";



                    



                    ApiResultModel<Registered> response = await client.PostAsyncRet<SignInViewModel, Registered>(this.SignInViewModel); //this._selectedImagePath == null ? new List<(Stream, string)>() : new List<(Stream, string)>() { (File.OpenRead(this._selectedImagePath), this.ImageFileName) }
                    if (response != null && response.Success && response.Data.RegisteredID != "0")
                    {
                        this.MainWindow = new MainWindow(response.Data.RegisteredID);
                        this.MainWindow.Show();
                        this.Close();

                    }
                    else if(response != null && response.Success)
                    {
                        this.ErrorMsg.Text = "User is banned.";
                        this.ErrorPanel.Visibility = Visibility.Visible;
                        this.isPasswordKeyDownEnabled = true;
                        this.SignInButton.IsEnabled = true;

                    }
                    else
                    {
                        //MessageBox.Show(
                        //            "The operation failed.",
                        //            "Error",
                        //            MessageBoxButton.OK,
                        //            MessageBoxImage.Error);
                        this.ErrorMsg.Text = "Failed, wrong password / UserName / Email.";
                        this.ErrorPanel.Visibility = Visibility.Visible;
                        this.isPasswordKeyDownEnabled = true;
                        this.SignInButton.IsEnabled = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                                    "Couldn't send the request due to network error on the host or the client.",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    this.isPasswordKeyDownEnabled = true;
                    this.SignInButton.IsEnabled = true;

                }
                
                
            }
            this.isPasswordKeyDownEnabled = true;
            this.SignInButton.IsEnabled = true;
        }

        public void RequiredPassword(object sender, RoutedEventArgs e)
        {
            string password = this.TxtPassword.Password;
            if (string.IsNullOrEmpty(password))
                this.ErrorPassword.Visibility = Visibility.Visible;
            else
            {
                this.ErrorPassword.Visibility = Visibility.Collapsed;
            }
        }

        //public void RequiredUserName(object sender, TextChangedEventArgs e)
        //{
        //    string UserName = this.SignInViewModel.SignKey;
        //    if (string.IsNullOrEmpty(UserName))
        //        this.ErrorUserName.Visibility = Visibility.Visible;
        //    else
        //    {
        //        this.ErrorUserName.Visibility = Visibility.Collapsed;
        //    }
        //}

        private void Close_click(object sender, RoutedEventArgs e) => this.Close();
        
    }
}
