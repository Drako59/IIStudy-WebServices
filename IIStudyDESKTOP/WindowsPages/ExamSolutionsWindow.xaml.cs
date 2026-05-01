using IIstudyWSClient;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ExamSolutionsWindow.xaml
    /// </summary>
    public partial class ExamSolutionsWindow : Window
    {

        private ObservableCollection<Solution> Solutions { get; set; }
        private ExamDetails Exam { get; set; }
        private CreateSolutionWindow CreateSolutionWindow { get; set; }
        private EditSolutionWindow EditSolutionWindow { get; set; }
        public ExamSolutionsWindow(ExamDetails exam)
        {
            InitializeComponent();
            this.Exam = exam;
            this.DataContext = this.Exam;
            init_page();
        }


        private async void init_page()
        {
            await this.LoadSolutions();
            this.UpdateStatistics();
        }

        private async Task LoadSolutions()
        {
            try
            {
                ApiClient<List<Solution>> client = new ApiClient<List<Solution>>();
                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5049;
                client.Path = "api/Guest/GetSolutions";
                client.AddParameter("examID", this.Exam.ExamID);
                List<Solution> solutions = await client.GetAsync();
                this.Solutions = new ObservableCollection<Solution>(solutions);

                if (this.Solutions == null)
                {
                    this.Solutions = new ObservableCollection<Solution>();
                    MessageBox.Show("Failed in reciving the books from web service", "Request Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }

                this.SolutionsList.ItemsSource = this.Solutions;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Faild to connect to host.", "Network Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateStatistics()
        {
            this.TxtSolutionCount.Text = this.Solutions?.Count().ToString() ?? "0";
        }

        private void CreateSolution(object sender, RoutedEventArgs e)
        {
            this.CreateSolutionWindow = new CreateSolutionWindow(this.Exam);
            this.CreateSolutionWindow.ShowDialog();

        }

        private void EditSolution(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Solution solution = btn.Tag as Solution;



            this.EditSolutionWindow = new EditSolutionWindow(solution,this.Exam);
            var result = this.EditSolutionWindow.ShowDialog();
            if (result == true)
                this.SolutionsList.Items.Refresh();
        }
        private void HardDeleteSolution(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Solution solution = btn.Tag as Solution;
            DeleteSolution(solution);

        }
        public async void DeleteSolution(Solution solution)
        {
            try
            {
                ApiClient<bool> client = new ApiClient<bool>();
                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5049;
                client.Path = "api/Admin/DeleteSolution";
                ApiResultModel<bool> result = await client.PostAsyncRet<Solution,bool>(solution);

                if (result == null || !result.Success || !result.Data)
                {
                    MessageBox.Show($"Failed in deleting solution #{solution.SolutionID}", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                this.Solutions.Remove(solution);

                this.SolutionsList.Items.Refresh();

                //this.SolutionsList.ItemsSource = null;
                //this.SolutionsList.ItemsSource = this.Solutions;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Faild to connect to host.", "Network Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
        private void Close_Click(object sender, RoutedEventArgs e) => Close();
    }
}
