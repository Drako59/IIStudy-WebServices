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
using System.Security.Permissions;
using System.Net.NetworkInformation;
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
        private CreateExamWindow CreateExamWindow { get; set; }
        private ExamSolutionsWindow ExamSolutionsWindow { get; set; }

        public ViewExams()
        {
            InitializeComponent();
            this.Init_page();
        }

        private async void Init_page()
        {
            await this.LoadExams();
            await this.LoadSubjects();
        }
        public async Task LoadExams()
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

        public async Task LoadSubjects()
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

        private async void SoftDeleteExam(Exam exam)
        {
            ApiClient<bool> client = new ApiClient<bool>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Admin/RemoveExam";
            ApiResultModel<bool> result = await client.PostAsyncRet<Exam, bool>(exam);

            if (result == null || !result.Success || !result.Data)
            {
                MessageBox.Show("Soft delete failed.", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                exam.IsDeleted = true;
                this.ExamsList.Items.Refresh();
            }
        }

        private async void RestoreExam(Exam exam)
        {
            ApiClient<bool> client = new ApiClient<bool>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Admin/RestoreExam";
            ApiResultModel<bool> result = await client.PostAsyncRet<Exam, bool>(exam);

            if(result == null || !result.Success || !result.Data)
            {
                MessageBox.Show("Restore failed.", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                exam.IsDeleted = false;
                this.ExamsList.Items.Refresh();
            }
        }

        public void ToggleDeleteButton(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Exam exam = btn.Tag as Exam;

            if (!exam.IsDeleted)
            {
                var confirm = MessageBox.Show($"Are you sure you want to temporary delete exam '{exam.Exam_Name}'?", "Validation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirm != MessageBoxResult.Yes) return;
                this.SoftDeleteExam(exam);
            }
            else if (exam.IsDeleted)
            {
                var confirm = MessageBox.Show($"Are you sure you want to  restore exam '{exam.Exam_Name}'?", "Validation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirm != MessageBoxResult.Yes) return;
                this.RestoreExam(exam);
            }
        }

        private void ViewExamSolutions(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ExamDetails exam = btn.Tag as ExamDetails;

            this.ExamSolutionsWindow = new ExamSolutionsWindow(exam);

            Window parentWindow = Window.GetWindow(this);
            this.ExamSolutionsWindow.Owner = parentWindow;
            this.ExamSolutionsWindow.Show();

        }
        public void ViewCreateExamWindow(object sender, RoutedEventArgs e)
        {

            this.CreateExamWindow = new CreateExamWindow(this.SubjectsDetails);

            this.CreateExamWindow.ShowDialog();



        }
    }
}
