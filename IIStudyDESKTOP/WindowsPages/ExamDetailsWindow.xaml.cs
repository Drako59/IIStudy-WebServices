using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using LLStudy_Models.ViewModels.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
    /// Interaction logic for 
    /// Window.xaml
    /// </summary>
    public partial class ExamDetailsWindow : Window
    {
        private EditExamWindow examDetailsWindow { get; set; }
        private ExamDetails Exam { get; set; }
        private List<SubjectDetails> SubjectsDetails { get; set; }
        public ExamDetailsWindow(ExamDetails exam,List<SubjectDetails> subjectsDetails)
        {
            InitializeComponent();
            
            MouseLeftButtonDown += (_, e) => { try { DragMove(); } catch { } };
            this.Exam = exam;
            this.SubjectsDetails = subjectsDetails;
            this.DataContext = this.Exam;
        }
        
        private void ViewEditExamWindow(object sender, RoutedEventArgs e)
        {
            var json = JsonSerializer.Serialize(this.Exam);
            var copy = JsonSerializer.Deserialize<ExamDetails>(json);

            this.examDetailsWindow = new EditExamWindow(this.Exam, this.SubjectsDetails);
            bool? response = this.examDetailsWindow.ShowDialog();
            
            
            if (response == true)
            {
                this.DataContext = null;
                this.DataContext = this.Exam;
            }
            else
            {
                this.Exam.Exam_Name = copy.Exam_Name;
                this.Exam.Exam_Year = copy.Exam_Year;
                this.Exam.File_path_url = copy.File_path_url;
                this.DataContext = this.Exam;
                this.DataContext = null;
                this.DataContext = this.Exam;
            }

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {

            Close();
        }
    }
}
