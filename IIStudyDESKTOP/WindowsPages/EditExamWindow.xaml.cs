using IIstudyWSClient;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using LLStudy_Models.ViewModels.Guest;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
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
    /// Interaction logic for EditExamWindow.xaml
    /// </summary>
    public partial class EditExamWindow : Window
    {
        private string _selectedPdfPath { get; set; }
        private string PdfFileName { get; set; }
        private ExamDetails Exam { get; set; }

        private List<SubjectDetails> SubjectDetails { get; set; }

        public EditExamWindow(ExamDetails exam, List<SubjectDetails> subjectDetails)
        {
            InitializeComponent();
            this.Exam = exam;
            this.DataContext = this.Exam;
            this.SubjectDetails = subjectDetails;
            this.InputSubjectID.ItemsSource = this.SubjectDetails;
            this.InputSubjectID.SelectedValue = this.Exam.SubjectID;
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
                this.Exam.File_path_url = this.PdfFileName;
                TxtSelectedFile.Foreground = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#4338ca"));
            }
            //this.ValidateAllFields();
        }

        private bool CheckValidation()
        {
            this.Exam.Validate();
            if (this.Exam.HasErrors)
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
            this.Exam.SubjectID = InputSubjectID.SelectedValue?.ToString() ?? "1";

            if (this.CheckValidation())
            {
                try
                {
                    ApiClient<bool> client = new ApiClient<bool>();
                    client.Scheme = "http";
                    client.Host = "localhost";
                    client.Port = 5049;
                    client.Path = "api/Admin/UpdateExam";

                    List<(Stream, string)> files_list = new List<(Stream, string)>();


                    if (this.PdfFileName != null)
                    {
                        files_list.Add((File.OpenRead(this._selectedPdfPath), this.PdfFileName));

                    }



                    ApiResultModel<Exam> response = await client.PostAsyncRet<Exam, Exam>(this.Exam, files_list); //this._selectedImagePath == null ? new List<(Stream, string)>() : new List<(Stream, string)>() { (File.OpenRead(this._selectedImagePath), this.ImageFileName) }
                    if (response.Success)
                    {

                        this.Exam.File_path_url = response.Data.File_path_url;
                        this.Exam.Subject_name = this.SubjectDetails.Where(s => s.SubjectID == response.Data.SubjectID).ToList().Select(s => s.Subject_name).ToList()[0]; //select the right name out of the subjectsNames list.
                        this.DataContext = this.Exam;
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
        private void ValidateAllFields() //Not In use
        {
            List<string> ProNames = new List<string>() { nameof(this.Exam.Exam_Name), nameof(this.Exam.Exam_Year) };

            this.Exam.ValidateAll();

            List<string> examNameErrors = this.Exam.GetErrors(nameof(this.Exam.Exam_Name))?.Cast<string>().ToList() ?? new List<string>();
            List<string> examYearErrors = this.Exam.GetErrors(nameof(this.Exam.Exam_Year))?.Cast<string>().ToList() ?? new List<string>();
            List<string> examFileErrors = this.Exam.GetErrors(nameof(this.Exam.File_path_url))?.Cast<string>().ToList() ?? new List<string>();


            if (examNameErrors.Any())
            {
                this.ErrorExamName.Visibility = Visibility.Visible;
                this.ErrorExamName.Text = $"ⓘ {examNameErrors[0]}";
            }
            else
            {
                this.ErrorExamName.Visibility = Visibility.Collapsed;
            }
            if (examYearErrors.Any())
            {
                this.ErrorExamYear.Visibility = Visibility.Visible;
                this.ErrorExamYear.Text = $"ⓘ {examYearErrors[0]}";
            }
            else
            {
                this.ErrorExamYear.Visibility = Visibility.Collapsed;
            }

            if (examFileErrors.Any())
            {
                this.ErrorExamFile.Visibility = Visibility.Visible;
                this.ErrorExamFile.Text = $"ⓘ {examFileErrors[0]}";
            }
            else
            {
                this.ErrorExamFile.Visibility = Visibility.Collapsed;
            }

            //Exam_Year
        }
        private void ValidateFields(object sender, TextChangedEventArgs e)
        {
            this.ValidateAllFields();
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
