using IIstudyWSClient;
using LLStudy_Models.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// Interaction logic for EditBook.xaml
    /// </summary>
    public partial class EditBook : Window
    {
        // !! UPDATE to your actual connection string !!
        

        private Book book;
        private string _selectedImagePath = null;
        private string _selectedPdfPath = null;
        private string ImageFileName = null;

        // Raised when save is successful so the parent window can refresh
        public event EventHandler<Book> OnSaved;

        public EditBook(Book book) 
        {
            this._selectedImagePath = null;
            InitializeComponent();
            // Allow dragging the borderless window
            MouseLeftButtonDown += (_, e) => { try { DragMove(); } catch { } };
            this.book = book;
            LoadBook();

            //PopulateFields(book);
        }

        // ════════════════════════════════════════════════════════════
        //  POPULATE FIELDS
        // ════════════════════════════════════════════════════════════

        private void LoadBook()
        {
            this.DataContext = this.book;
            this.CheckIfImageExist();

        }
        private void PopulateFields(Book book)
        {
            TxtSubtitle.Text = $"Editing: {book.Book_name}";
            EditBookName.Text = book.Book_name ?? "";
            EditAuthorName.Text = book.Author_name ?? "";
            EditPrice.Text = book.Book_price != null
                                        ? book.Book_price.ToString("N2") : "";
            EditSubjectID.Text = book.SubjectID ?? "";
            //EditType.Text = book.Type ?? "";
            //EditBookAuthor.Text = book.Author_name ?? book.Author_name ?? "";
            EditInStock.IsChecked = book.In_stock;

            if (!string.IsNullOrWhiteSpace(book.Pdf_url_book) &&
                book.Pdf_url_book != "No PDF available")
                TxtPdfPath.Text = book.Pdf_url_book;

            // Load existing cover image
            if (!string.IsNullOrWhiteSpace(book.BookImagePath) && book.BookImagePath != "None")
            {
                TxtImagePath.Text = System.IO.Path.GetFileName(book.BookImagePath);
                TryShowCover(book.BookImagePath);
            }
        }

        // ════════════════════════════════════════════════════════════
        //  COVER IMAGE DISPLAY
        // ════════════════════════════════════════════════════════════
        private void TryShowCover(string path)
        {
            try
            {
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;

                if (path.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    bmp.UriSource = new Uri(path);
                else if (File.Exists(path))
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

        // ── Hover overlay on cover ───────────────────────────────────
        private void Cover_MouseEnter(object sender, MouseEventArgs e)
            => CoverOverlay.Visibility = Visibility.Visible;

        private void Cover_MouseLeave(object sender, MouseEventArgs e)
            => CoverOverlay.Visibility = Visibility.Collapsed;

        private void CoverOverlay_Click(object sender, MouseButtonEventArgs e)
            => BrowseImage_Click(sender, null);

        // ════════════════════════════════════════════════════════════
        //  BROWSE — IMAGE
        // ════════════════════════════════════════════════════════════
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

            if (dlg.ShowDialog() == true)
            {
                this._selectedImagePath = dlg.FileName;
                this.ImageFileName = System.IO.Path.GetFileName(dlg.FileName);
                TxtImagePath.Text = this.ImageFileName;
                TxtImagePath.Foreground = Brushes.White;
                TryShowCover(dlg.FileName);
            }
        }

        // ════════════════════════════════════════════════════════════
        //  BROWSE — PDF
        // ════════════════════════════════════════════════════════════
        private void BrowsePdf_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Title = "Select PDF File",
                Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*"
            };

            if (dlg.ShowDialog() == true)
            {
                _selectedPdfPath = dlg.FileName;
                TxtPdfPath.Text = System.IO.Path.GetFileName(dlg.FileName);
                TxtPdfPath.Foreground = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#4338ca"));
            }
        }

        // ════════════════════════════════════════════════════════════
        //  SAVE
        // ════════════════════════════════════════════════════════════
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EditBookName.Text))
            {
                MessageBox.Show("Book name cannot be empty.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(EditPrice.Text, out double newPrice))
            {
                MessageBox.Show("Please enter a valid numeric price.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string finalImagePath = _selectedImagePath ?? book.BookImagePath;
            string finalPdfPath = _selectedPdfPath ?? book.Pdf_url_book;

            try
            {
                

                

                // Update in-memory model
                this.book.Book_name = EditBookName.Text.Trim();
                this.book.Author_name = EditAuthorName.Text.Trim();
                this.book.Book_price = newPrice;
                this.book.SubjectID = EditSubjectID.Text.Trim();
                //this._book.Type = EditType.Text.Trim();
                //this.book.Author_name = EditBookAuthor.Text.Trim();
                this.book.In_stock = EditInStock.IsChecked == true;
                this.book.Pdf_url_book = finalPdfPath;
                this.book.BookImagePath = finalImagePath;

                MessageBox.Show("Book updated successfully! ✅", "Saved",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                OnSaved?.Invoke(this, book);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Save failed:\n{ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ════════════════════════════════════════════════════════════
        //  BACK / CANCEL
        // ════════════════════════════════════════════════════════════
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private async void UpdateBook(object sender,RoutedEventArgs e)
        {
            ApiClient<bool> client = new ApiClient<bool>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Admin/UpdateFullBook";
            ApiResultModel<Book> response = await client.PostAsyncRet<Book,Book>(this.book,this._selectedImagePath ==  null? new List<(Stream, string)>() : new List<(Stream,string)>() { (File.OpenRead(this._selectedImagePath), this.ImageFileName) });
            if (response.Success)
            {
                this.book.BookImagePath = response.Data.BookImagePath;
                this.DataContext = this.book;
                this.DialogResult = true;
                this.Close();
                this.CheckIfImageExist();
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

        private void CheckIfImageExist()
        {
            if(this.book.BookImagePath.ToLower() != "none" && this.book.BookImagePath != null)
            {
                CoverImage.Visibility = Visibility.Visible;
                CoverEmoji.Visibility = Visibility.Collapsed;
            }
            else
            {
                CoverImage.Visibility = Visibility.Collapsed;
                CoverEmoji.Visibility = Visibility.Visible;
            }
        }

    }
}
