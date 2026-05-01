using IIstudyWSClient;
using LLStudy_Models.Models;
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
using System.Windows.Shapes;
using LLStudy_Models.ViewModels;

namespace IIStudyDESKTOP.WindowsPages
{
    /// <summary>
    /// Interaction logic for ExamSolutionsWindow.xaml
    /// </summary>
    public partial class ExamSolutionsWindow : Window
    {

        private List<Solution> Solutions { get; set; }
        private ExamDetails Exam { get; set; }
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
                this.Solutions = await client.GetAsync();

                if (this.Solutions == null)
                {
                    this.Solutions = new List<Solution>();
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

        private void Close_Click(object sender, RoutedEventArgs e) => Close();
    }
}
