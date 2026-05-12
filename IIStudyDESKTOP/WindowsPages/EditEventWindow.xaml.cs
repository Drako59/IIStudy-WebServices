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
    /// Interaction logic for EditEventWindow.xaml
    /// </summary>
    public partial class EditEventWindow : Window
    {
        public EditEventWindow()
        {
            InitializeComponent();
        }

        private void Close_click(object sedner, RoutedEventArgs e) => this.Close();
    }
}
