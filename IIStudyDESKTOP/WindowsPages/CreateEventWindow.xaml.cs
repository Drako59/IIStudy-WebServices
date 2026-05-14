using IIstudyWSClient;
using LLStudy_Models.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace IIStudyDESKTOP.WindowsPages
{
    /// <summary>
    /// Interaction logic for CreateEventWindow.xaml
    /// </summary>
    public partial class CreateEventWindow : Window
    {
        private Event Event { get; set; }
        public CreateEventWindow()
        {
            InitializeComponent();
            this.Event = new Event()
            {
                EventID = "0"
            };
            this.DataContext = this.Event;
        }

        private bool CheckValidation()
        {
            this.Event.Validate();
            if (this.Event.HasErrors)
            {
                MessageBox.Show(
                               "One or more field are not as requested",
                               "Error",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private async void CreateEvent(object sender, RoutedEventArgs e)
        {
            if (this.CheckValidation())
            {
                try
                {
                    ApiClient<bool> client = new ApiClient<bool>();
                    client.Scheme = "http";
                    client.Host = "localhost";
                    client.Port = 5049;
                    client.Path = "api/Admin/CreateEvent";







                    ApiResultModel<bool> response = await client.PostAsyncRet<Event, bool>(this.Event); //this._selectedImagePath == null ? new List<(Stream, string)>() : new List<(Stream, string)>() { (File.OpenRead(this._selectedImagePath), this.ImageFileName) }
                    if (response.Success && response.Data)
                    {
                        this.DialogResult = true;
                        this.Close();

                    }
                    else
                    {
                        MessageBox.Show(
                                    "The operation failed.",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    }
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
        }

        private void Close_click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
