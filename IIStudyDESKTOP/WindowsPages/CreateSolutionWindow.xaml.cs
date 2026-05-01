using IIstudyWSClient;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using LLStudy_Models.ViewModels.Guest;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for CreateSolutionWindow.xaml
    /// </summary>
    public partial class CreateSolutionWindow : Window
    {
        private string _selectedPdfPath { get; set; }
        private string PdfFileName { get; set; }
        private Solution Solution { get; set; }

        

        public CreateSolutionWindow(ExamDetails exam)
        {
            InitializeComponent();
            this.Solution = new Solution() { SolutionID = "0", File_path_url = "None", ExamID = exam.ExamID };
            this.DataContext = this.Solution;
            this.TxtExamName.Text = exam.Exam_Name;
            this.TxtExamID.Text = $"ExamID #{exam.ExamID}";
            
            //this.ValidateAllFields();
        }

        private void BrowsePdf_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Title = "Select PDF File",
                Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*"
            };

            if (dlg.ShowDialog() == true)
            {
                this._selectedPdfPath = dlg.FileName;
                this.PdfFileName = System.IO.Path.GetFileName(dlg.FileName);
                TxtSelectedFile.Foreground = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#4338ca"));
                this.Solution.File_path_url = this.PdfFileName;
                TxtSelectedFile.Text = this.Solution.File_path_url;
                TxtCurrentFile.Text = this.Solution.File_path_url;
            }
            //this.ValidateAllFields();
        }

        private bool CheckValidation()
        {
            this.Solution.Validate();
            if (this.Solution.HasErrors)
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

        private async void CreateSolution(object sender, RoutedEventArgs e)
        {
            //if (this.BtnSaveValidationClick(sender, e))
            //this.Exam.SubjectID = this.InputSubject.ToString();
            //this.Exam.Exam_Name = this.InputExamName.ToString();
            //this.Exam.Exam_Year = this.InputExamYear.ToString();
           
            if (this.CheckValidation())
            {
                try
                {
                    ApiClient<bool> client = new ApiClient<bool>();
                    client.Scheme = "http";
                    client.Host = "localhost";
                    client.Port = 5049;
                    client.Path = "api/Admin/CreateNewSolution";

                    List<(Stream, string)> files_list = new List<(Stream, string)>();


                    if (this.PdfFileName != null)
                    {
                        files_list.Add((File.OpenRead(this._selectedPdfPath), this.PdfFileName));

                    }



                    ApiResultModel<bool> response = await client.PostAsyncRet<Solution, bool>(this.Solution, files_list); //this._selectedImagePath == null ? new List<(Stream, string)>() : new List<(Stream, string)>() { (File.OpenRead(this._selectedImagePath), this.ImageFileName) }
                    if (response.Success)
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
            this.Close();
        }

        private void InputExamName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
