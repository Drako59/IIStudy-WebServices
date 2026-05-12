using IIStudyDESKTOP.WindowsPages;
using IIstudyWSClient;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using LLStudy_Models.ViewModels.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace IIStudyDESKTOP.UserControllers
{
    /// <summary>
    /// Interaction logic for ViewExams.xaml
    /// </summary>
    /// 
    


    public partial class ViewExams : UserControl
    {
        private List<ExamDetails> Exams { get; set; }
        private List<SubjectDetails> SubjectsDetails { get; set; }

        private ExamDetailsWindow ExamWindow { get; set; }
        private CreateExamWindow CreateExamWindow { get; set; }
        private ExamSolutionsWindow ExamSolutionsWindow { get; set; }
        private SubjectsWindow SubjectsWindow { get; set; }
        private FilterStatus FilterStatus { get; set; } = FilterStatus.All;
        private string SearchText { get; set; } = "";
        private string SelectedSubject { get; set; } = "0";
        private string SearchSubjectText { get; set; } = "";


        public ViewExams()
        {
            InitializeComponent();
            this.Init_page();
        }

        private async void Init_page()
        {
            await this.LoadExams();
            await this.LoadSubjects();
            this.UpdateStatistics();
        }
        private void UpdateStatistics()
        {
            this.TxtRestricted.Text = this.Exams?.Where(e => e.IsDeleted).ToList().Count().ToString() ?? "0";
            this.TxtActive.Text = this.Exams?.Where(e => !e.IsDeleted).ToList().Count().ToString() ?? "0";
            this.TxtTotalExams.Text = this.Exams?.Count().ToString() ?? "0";
            this.TxtSubjectCount.Text = this.SubjectsDetails?.Count().ToString() ?? "0";
            this.TxtSubjectBadge.Text = this.TxtSubjectCount.Text;

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
            client.Path = "api/Guest/GetExamsSubjectsDetails";
            this.SubjectsDetails = await client.GetAsync();
            if (this.SubjectsDetails == null)
            {
                MessageBox.Show("Faild to Load subjects", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.SubjectsDetails = new List<SubjectDetails>();
            }

            this.SubjectsDetails.Insert(0, new SubjectDetails() { SubjectCounter = this.Exams.Count(), SubjectID = "0", Subject_name = "All" });

            this.SubjectsList.ItemsSource = this.SubjectsDetails;
            
        }

        public void ViewExamDetailsWindow(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ExamDetails exam = btn.Tag as ExamDetails;

            this.ExamWindow = new ExamDetailsWindow(exam, SubjectsDetails);

            Window parentWindow = Window.GetWindow(this);
            this.ExamWindow.Owner = parentWindow;
            
            this.ExamWindow.Closed += (s, args) =>
            {
                parentWindow.Activate();
            };

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

            this.ExamSolutionsWindow.Closed += (s, args) =>
            {
                parentWindow.Activate();
            };

            this.ExamSolutionsWindow.Show();

        }
        public void ViewCreateExamWindow(object sender, RoutedEventArgs e)
        {

            this.CreateExamWindow = new CreateExamWindow(this.SubjectsDetails);

            this.CreateExamWindow.ShowDialog();



        }

        private void SetSelectedChip(Border active)
        {
            // ── Reset all chips to their inactive (muted) look ──────────

            // ChipAll — blue gradient when active, transparent when not
            // RESET
            ChipAll.Background = new SolidColorBrush(Colors.Transparent);
            TxtAllChip.Foreground = new LinearGradientBrush(
                    (Color)ColorConverter.ConvertFromString("#5b5fcf"),
                    (Color)ColorConverter.ConvertFromString("#1976d2"),
                    new Point(0, 0.5), new Point(1, 0.5));
            ChipActive.Background = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#f0fdf4"));
            ChipDeleted.Background = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#fef2f2"));
            

            // ── Highlight the active chip ────────────────────────────────
            active.Opacity = 1.0;

            if (active == ChipAll)
            {
                ChipAll.Background = new LinearGradientBrush(
                    (Color)ColorConverter.ConvertFromString("#5b5fcf"),
                    (Color)ColorConverter.ConvertFromString("#1976d2"),
                    new Point(0, 0.5), new Point(1, 0.5));
                TxtAllChip.Foreground = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#ffffff"));
            }
            else if (active == ChipActive)
            {
                ChipActive.Background = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#16a34a"));
            }
            else if (active == ChipDeleted)
            {
                ChipDeleted.Background = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#dc2626"));
            }
        }

        private void Filter()
        {
            

            
            List<ExamDetails> filtered;
            switch (this.FilterStatus)
            {
                case FilterStatus.Active:
                    filtered = this.Exams.Where(e => !e.IsDeleted).ToList();
                    SetSelectedChip(this.ChipActive);
                    break;
                case FilterStatus.Deleted:
                    filtered = this.Exams.Where(e => e.IsDeleted).ToList();
                    SetSelectedChip(this.ChipDeleted);
                    break;
                default:
                    filtered = this.Exams;
                    SetSelectedChip(this.ChipAll);
                    break;
            }
            filtered = filtered.Where(e => 
            {
                bool searchBox = e.Exam_Name.ToLower().Contains(this.SearchText.ToLower());
                bool subjectFilter = this.SelectedSubject == "0" ? true : e.SubjectID == this.SelectedSubject;
                return searchBox && subjectFilter;
            }).ToList();
            this.ExamsList.ItemsSource = null;
            this.ExamsList.ItemsSource = filtered;


        }

        private void SetFilterChip(object sender, MouseButtonEventArgs e)
        {
            Border btn = sender as Border;
            int status = int.Parse(btn.Tag.ToString());

            switch (status)
            {
                case 0:
                    this.FilterStatus = FilterStatus.All;
                    break;
                case 1:
                    this.FilterStatus = FilterStatus.Active;
                    break;
                case 2:
                    this.FilterStatus = FilterStatus.Deleted;
                    break;

            }
            this.Filter();

        }

        private void SearchTextUpadte(object sender, TextChangedEventArgs e)
        {
            this.SearchText = this.SearchBox.Text;
            this.Filter();
        }

        private void SubjectFilter(object sender, MouseButtonEventArgs e)
        {
            Border btn = sender as Border;
            string subjectID = btn.Tag.ToString();
            this.SelectedSubject = subjectID;
            this.Filter();
        }
        private void SearchSubject(object sender, TextChangedEventArgs e)
        {
            this.SearchSubjectText = this.SearchSubjectTxtBox.Text;
            List<SubjectDetails> filteredSubjects = this.SubjectsDetails.Where(s => {
                bool name = s.Subject_name.ToLower().Contains(this.SearchSubjectText.ToLower());
                bool subjectID = s.SubjectID == this.SearchSubjectText;
                return name || subjectID;
            }).ToList();
            this.SubjectsList.ItemsSource = null;
            this.SubjectsList.ItemsSource = filteredSubjects;
        }

        private void ViewSubjects(object sender, RoutedEventArgs e)
        {
            this.SubjectsWindow = new SubjectsWindow();
            Window parentWindow = Window.GetWindow(this);
            this.SubjectsWindow.Owner = parentWindow;
            this.SubjectsWindow.Show();
        }

        
    }
}
