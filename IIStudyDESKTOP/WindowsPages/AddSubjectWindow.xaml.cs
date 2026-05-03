using IIstudyWSClient;
using LLStudy_Models.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for AddSubjectWindow.xaml
    /// </summary>
    public partial class AddSubjectWindow : Window
    {
        private Subject Subject { get; set; }
        public AddSubjectWindow()
        {
            InitializeComponent();
            this.Subject = new Subject { SubjectID = "0"};

            this.DataContext = this.Subject;
        }

        private bool CheckValidation()
        {
            this.Subject.Validate();
            if (this.Subject.HasErrors)
            {
                MessageBox.Show(
                               "The field must be as requested, up to 20 characters.",
                               "Error",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private async void AddSubject(object sender, RoutedEventArgs e)
        {
            if (this.CheckValidation())
            {
                try
                {
                    ApiClient<bool> client = new ApiClient<bool>();
                    client.Scheme = "http";
                    client.Host = "localhost";
                    client.Port = 5049;
                    client.Path = "api/Admin/CreateSubject";

                    ApiResultModel<bool> response = await client.PostAsyncRet<Subject, bool>(this.Subject);

                    if (response == null || !response.Success || !response.Data)
                    {
                        this.DialogResult = false;
                        MessageBox.Show("Failed in adding new subject", "Request Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;

                    }

                    this.DialogResult = true;
                    this.Close();



                }
                catch (Exception ex)
                {
                    this.DialogResult = false;
                    MessageBox.Show("Faild to connect to host.", "Network Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
