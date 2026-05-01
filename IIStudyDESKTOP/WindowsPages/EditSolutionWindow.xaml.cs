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
    /// Interaction logic for EditSolutionWindow.xaml
    /// </summary>
    public partial class EditSolutionWindow : Window
    {
        private string _selectedPdfPath { get; set; }
        private string PdfFileName { get; set; }
        private Solution Solution { get; set; }

        private Solution OriginalSolution { get; set; }
       

        public EditSolutionWindow(Solution solution, ExamDetails exam)
        {
            InitializeComponent();
            var json = JsonSerializer.Serialize(solution);
            var copy = JsonSerializer.Deserialize<Solution>(json);
            this.OriginalSolution = solution;
            this.Solution = copy;
            this.DataContext = this.Solution;
            this.TxtExamID.Text = $"ExamID #{exam.ExamID}";
            this.TxtExamName.Text = exam.Exam_Name;

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
                this.Solution.File_path_url = this.PdfFileName;
                TxtSelectedFile.Foreground = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#4338ca"));
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

        private async void UpdateExam(object sender, RoutedEventArgs e)
        {
            //if (this.BtnSaveValidationClick(sender, e))
            

            if (this.CheckValidation())
            {
                try
                {
                    ApiClient<bool> client = new ApiClient<bool>();
                    client.Scheme = "http";
                    client.Host = "localhost";
                    client.Port = 5049;
                    client.Path = "api/Admin/UpdateSolution";

                    List<(Stream, string)> files_list = new List<(Stream, string)>();


                    if (this.PdfFileName != null)
                    {
                        files_list.Add((File.OpenRead(this._selectedPdfPath), this.PdfFileName));

                    }



                    ApiResultModel<Solution> response = await client.PostAsyncRet<Solution, Solution>(this.Solution, files_list); //this._selectedImagePath == null ? new List<(Stream, string)>() : new List<(Stream, string)>() { (File.OpenRead(this._selectedImagePath), this.ImageFileName) }
                    if (response.Success)
                    {

                        this.Solution.File_path_url = response.Data.File_path_url;
                        
                        this.DataContext = this.Solution;
                        this.DialogResult = true;

                        this.OriginalSolution.ExamID = this.Solution.ExamID;
                        this.OriginalSolution.SolutionID = this.Solution.SolutionID;
                        this.OriginalSolution.Solution_Name = this.Solution.Solution_Name;
                        this.OriginalSolution.Solution_Year = this.Solution.Solution_Year;
                        this.OriginalSolution.File_path_url = this.Solution.File_path_url;


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

     
    }
}
