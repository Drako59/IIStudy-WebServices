using IIstudyWSClient;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using LLStudy_Models.ViewModels.Guest;
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
using IIStudyDESKTOP.WindowsPages;
namespace IIStudyDESKTOP.UserControllers
{
    /// <summary>
    /// Interaction logic for ViewExams.xaml
    /// </summary>
    public partial class ViewExams : UserControl
    {
        private List<ExamDetails> Exams { get; set; }
        private List<SubjectDetails> SubjectsDetails { get; set; }

        private ExamDetailsWindow ExamWindow { get; set; }

        public ViewExams()
        {
            InitializeComponent();
            Loaded += (_, __) => { LoadExams(); LoadSubjects(); };
        }

        public async void LoadExams()
        {
            ApiClient<List<ExamDetails>> client = new ApiClient<List<ExamDetails>>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/GetExams";
            this.Exams = await client.GetAsync();
            if (this.Exams == null)
            {
                MessageBox.Show("Faild to Load Exams", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.SubjectsDetails = new List<SubjectDetails>();
            }
            this.ExamsList.ItemsSource = this.Exams;
            

        }

        public async void LoadSubjects()
        {
            ApiClient<List<SubjectDetails>> client = new ApiClient<List<SubjectDetails>>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Guest/GetAllSubjectsDetails";
            this.SubjectsDetails = await client.GetAsync();
            if (this.SubjectsDetails == null)
            {
                MessageBox.Show("Faild to Load subjects", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.SubjectsDetails = new List<SubjectDetails>();
            }

            this.SubjectsDetails.Insert(0, new SubjectDetails() { BooksCount = 0, SubjectID = "0", Subject_name = "All" });

            this.SubjectsList.ItemsSource = this.SubjectsDetails;
        }

        public void ViewExamDetailsWindow(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ExamDetails exam = btn.Tag as ExamDetails;

            this.ExamWindow = new ExamDetailsWindow(exam, SubjectsDetails);

            Window parentWindow = Window.GetWindow(this);
            this.ExamWindow.Owner = parentWindow;
            this.ExamWindow.Show();



        }
    }
}
