using IIstudyWSClient;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels.Guest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
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
        private EditSubjectWindow EditSubjectWindow { get; set; }
        private string SearchSubjectText { get; set; } = "";
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

        private async void EditSubject(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Subject subject = btn.Tag as Subject;

            var json = JsonSerializer.Serialize(subject);
            Subject copy = JsonSerializer.Deserialize<Subject>(json);

            this.EditSubjectWindow = new EditSubjectWindow(subject);
            bool? result = this.EditSubjectWindow.ShowDialog();

            if(result != true)
            {
                subject.Subject_name = copy.Subject_name;
                this.SubjectsList.Items.Refresh();
            }
        }

        private void SearchSubject(object sender, TextChangedEventArgs e)
        {
            this.SearchSubjectText = this.SearchBox.Text;
            ObservableCollection<Subject> filteredSubjects = new ObservableCollection<Subject>(this.Subjects.Where(s => {
                bool name = s.Subject_name.ToLower().Contains(this.SearchSubjectText.ToLower());
                bool subjectID = s.SubjectID == this.SearchSubjectText;
                return name || subjectID;
                }));
            this.SubjectsList.ItemsSource = null;
            this.SubjectsList.ItemsSource = filteredSubjects;
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();
        

        

        private async void RefreshSubjects()
        {
            await this.GetSubjects();
        }
    }
}
