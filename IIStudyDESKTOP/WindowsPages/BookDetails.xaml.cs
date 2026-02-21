using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;
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
using IIStudyDESKTOP.WindowsPages;
namespace IIStudyDESKTOP.WindowsPages
{
    /// <summary>
    /// Interaction logic for BookDetails.xaml
    /// </summary>
    public partial class BookDetails : Window
    {
        EditBook editBook;
        public BookDetails()
        {
            InitializeComponent();
        }

        private void  ViewEditBook(object sender, RoutedEventArgs e) 
        {
            //if (this.editBook == null)
            this.editBook = new EditBook();

            this.editBook.Owner = this;
            this.editBook.ShowDialog() ;
        }
    }
}
