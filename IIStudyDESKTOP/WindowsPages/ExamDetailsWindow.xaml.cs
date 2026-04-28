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

namespace IIStudyDESKTOP.WindowsPages
{
    /// <summary>
    /// Interaction logic for ExamDetailsWindow.xaml
    /// </summary>
    public partial class ExamDetailsWindow : Window
    {
        private Exam Exam { get; set; }
        public ExamDetailsWindow(Exam exam)
        {
            InitializeComponent();
            MouseLeftButtonDown += (_, e) => { try { DragMove(); } catch { } };
            this.Exam = exam;
            this.DataContext = this.Exam;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {

            Close();
        }
    }
}
