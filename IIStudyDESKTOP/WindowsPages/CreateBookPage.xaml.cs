using IIstudyWSClient;
using LLStudy_Models.Models;
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
    /// Interaction logic for CreateBookPage.xaml
    /// </summary>
    public partial class CreateBookPage : Window
    {

        // !! UPDATE to your actual connection string !!
        private string fileName = null;
        private string ImageFileName = null;
        private string _selectedImagePath = null;
        private string _selectedPdfPath = null;
        private Dictionary<string,string> Subjects { get; set; }
        // Raised after a successful insert so the caller can refresh its book list
        public event EventHandler<Book> OnBookCreated;

        public CreateBookPage(Dictionary<string, string> subjects)
        {
            InitializeComponent();
            this.Subjects = subjects;
            this.InputSubjectID.ItemsSource = this.Subjects;
            MouseLeftButtonDown += (_, e) => { try { DragMove(); } catch { } };
        }

        // ════════════════════════════════════════════════════════════
        //  COVER IMAGE — hover & browse
        // ════════════════════════════════════════════════════════════
        private void Cover_MouseEnter(object sender, MouseEventArgs e)
            => CoverOverlay.Visibility = Visibility.Visible;

        private void Cover_MouseLeave(object sender, MouseEventArgs e)
            => CoverOverlay.Visibility = Visibility.Collapsed;

        private void CoverOverlay_Click(object sender, MouseButtonEventArgs e)
            => BrowseImage_Click(sender, null);

        private void BrowseImage_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Title = "Select Book Cover Image",
                Filter = "Image Files|*.png;*.jpg;*.jpeg;*.webp;*.gif|" +
                            "PNG (*.png)|*.png|" +
                            "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                            "WebP (*.webp)|*.webp|" +
                            "GIF (*.gif)|*.gif|" +
                            "All Files (*.*)|*.*"
            };

            if (dlg.ShowDialog() != true) return;

            _selectedImagePath = dlg.FileName;
            this.ImageFileName = System.IO.Path.GetFileName(dlg.FileName);
            TxtImagePath.Text = this.ImageFileName;
            TxtImagePath.Foreground = Brushes.White;
            TryShowCover(dlg.FileName);
        }

        private void TryShowCover(string path)
        {
            try
            {
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;

                if (path.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    bmp.UriSource = new Uri(path);
                else if (System.IO.File.Exists(path))
                    bmp.UriSource = new Uri(path, UriKind.Absolute);
                else return;

                bmp.EndInit();
                CoverImage.Source = bmp;
                CoverImage.Visibility = Visibility.Visible;
                CoverEmoji.Visibility = Visibility.Collapsed;
            }
            catch
            {
                CoverImage.Visibility = Visibility.Collapsed;
                CoverEmoji.Visibility = Visibility.Visible;
            }
        }

        // ════════════════════════════════════════════════════════════
        //  PDF BROWSE
        // ════════════════════════════════════════════════════════════
        private void BrowsePdf_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Title = "Select PDF File",
                Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*"
            };

            if (dlg.ShowDialog() != true) return;

            _selectedPdfPath = dlg.FileName;

            this.fileName = System.IO.Path.GetFileName(dlg.FileName);
            TxtPdfPath.Text = this.fileName;
            TxtPdfPath.Foreground = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#4338ca"));
        }

        // ════════════════════════════════════════════════════════════
        //  CREATE — validate + INSERT
        // ════════════════════════════════════════════════════════════
        private async void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            // ── Validation ──────────────────────────────────────────
            if (string.IsNullOrWhiteSpace(InputBookName.Text))
            {
                Shake(InputBookName);
                MessageBox.Show("Book name is required.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(InputAuthorName.Text))
            {
                Shake(InputAuthorName);
                MessageBox.Show("Author name is required.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(InputPrice.Text, out double price))
            {
                Shake(InputPrice);
                MessageBox.Show("Please enter a valid numeric price.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // ── Insert ──────────────────────────────────────────────
            try
            {


                // Get the new BookID back

                // Build the new Book object and raise event
                var newBook = new Book
                {
                    BookID = "0",
                    Book_name = InputBookName.Text.Trim(),
                    Author_name = InputAuthorName.Text.Trim(),
                    Book_price = price,
                    SubjectID = InputSubjectID.SelectedValue.ToString(),
                    IsOnline = this.InputType.IsChecked == true,
                    //BookAuthor = InputBookAuthor.Text.Trim(),
                    In_stock = InputInStock.IsChecked == true,
                    Pdf_url_book = _selectedPdfPath == null ? "None" : this._selectedPdfPath,
                    BookImagePath = _selectedImagePath
                };

                OnBookCreated?.Invoke(this, newBook);



                

                ApiClient<Book> client = new ApiClient<Book>();
                ApiResultModel<bool> result = new ApiResultModel<bool>();
                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5049;
                client.Path = "api/Admin/CreateNewBook";

                List<(Stream, string)> files_list = new List<(Stream, string)>();

                if (this._selectedImagePath != null)
                {
                    files_list.Add( (File.OpenRead(this._selectedImagePath), this.ImageFileName) );
                }
                if (this._selectedPdfPath != null)
                {
                    files_list.Add(  (File.OpenRead(this._selectedPdfPath), this.fileName ) );
                }



                result = await client.PostAsyncRet<Book,bool>(newBook, files_list); //this._selectedImagePath == null ? new List<(Stream, string)>() : new List<(Stream, string)>() { (File.OpenRead(this._selectedImagePath), this.ImageFileName) }
                if (!result.Success || !result.Data)
                {
                    DialogResult = false;
                    
                    throw new Exception(message: "Failed to upload image");
                }
                
                MessageBox.Show($"✅  \"{newBook.Book_name}\" was added successfully!",
                    "Book Created", MessageBoxButton.OK, MessageBoxImage.Information); 
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create book:\n{ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ── Highlight the invalid field border briefly ───────────────
        private void Shake(System.Windows.Controls.TextBox box)
        {
            box.BorderBrush = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#ef4444"));
            box.BorderThickness = new Thickness(2);

            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1.5)
            };
            timer.Tick += (_, __) =>
            {
                box.BorderBrush = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#e2e8f0"));
                box.BorderThickness = new Thickness(1.5);
                timer.Stop();
            };
            timer.Start();
        }

        // ════════════════════════════════════════════════════════════
        //  CANCEL / CLOSE
        // ════════════════════════════════════════════════════════════
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
