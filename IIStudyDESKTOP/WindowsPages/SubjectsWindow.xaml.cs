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
    /// Interaction logic for SubjectsWindow.xaml
    /// </summary>
    public partial class SubjectsWindow : Window
    {
        private ObservableCollection<Subject> Subjects { get; set; }
        private AddSubjectWindow AddSubjectWindow { get; set; }
        public SubjectsWindow()
        {
            InitializeComponent();

            this.Init_window();
        }
        private async void Init_window()
        {
            await this.GetSubjects();
        }

        private async Task GetSubjects()
        {
            try
            {
                ApiClient<List<Subject>> client = new ApiClient<List<Subject>>();
                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5049;
                client.Path = "api/Guest/GetSubjects";
                
                List<Subject> subjects = await client.GetAsync();

                if (subjects == null)
                {
                    this.Subjects = new ObservableCollection<Subject>();
                    MessageBox.Show("Failed in reciving the subjects from web service", "Request Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                this.Subjects = new ObservableCollection<Subject>(subjects);


                this.SubjectsList.ItemsSource = this.Subjects;
                this.TxtSubjectCount.Text = this.Subjects.Count().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Faild to connect to host.", "Network Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void AddSubject(object sender, RoutedEventArgs e)
        {
            this.AddSubjectWindow = new AddSubjectWindow();

            bool? result = this.AddSubjectWindow.ShowDialog();

            if (result == true)
                this.RefreshSubjects();
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();
        

        

        private async void RefreshSubjects()
        {
            await this.GetSubjects();
        }
    }
}
