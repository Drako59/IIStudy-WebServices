using IIstudyWSClient;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for RegisteredDetails.xaml
    /// </summary>
    public partial class RegisteredDetails : Window
    {
        private Registered reg;
        private List<RegisteredComments> regComments;

        private static readonly Dictionary<string, string[]> RoleStyle = new()
        {
            { "admin",   new[] { "#7c3aed", "#ede9fe", "👑 Admin"   } },
            /*{ "manager", new[] { "#b45309", "#fef3c7", "🛠 Manager"  } },*/
            { "user",    new[] { "#0369a1", "#e0f2fe", "👤 User"     } },
        };

        public RegisteredDetails(Registered reg)
        {
            this.reg = reg;
            InitializeComponent();
            MouseLeftButtonDown += (_, e) => { try { DragMove(); } catch { } };
            LoadRegistered();
            LoadRegisteredReview();
        }



        void LoadRegistered()
        {
            TxtInitials.Text = this.reg.UserName.Length >= 2
                   ? $"{char.ToUpper(this.reg.UserName[0])}{char.ToUpper(this.reg.UserName[1])}"
                   : char.ToUpper(this.reg.UserName[0]).ToString();

            // Role badge
            if (RoleStyle.TryGetValue(this.reg.Role.ToLower(), out var rs))
            {
                RoleBadge.Background = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString(rs[0]));
                TxtRoleBadge.Text = rs[2];
            }
            else
            {
                RoleBadge.Background = new SolidColorBrush(Colors.Gray);
                TxtRoleBadge.Text = $"👤 {this.reg.Role}";
            }

            // Banned badge
            if (this.reg.IsBanned)
            {
                BannedBadge.Visibility = Visibility.Visible;
                BannedBadge.Background = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#dc2626"));
            }
            this.DataContext = reg;
        }

        async void  LoadRegisteredReview()
        {
            ApiClient<List<RegisteredComments>> client = new ApiClient<List<RegisteredComments>>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Admin/GetRegisteredReviews";
            client.AddParameter("registeredID", this.reg.RegisteredID);
            this.regComments = await client.GetAsync();
            if (!this.regComments.Any()) return;
            TxtReviewCount.Text = this.regComments.Count.ToString();
            EmptyReviews.Visibility = this.regComments.Count == 0
                ? Visibility.Visible : Visibility.Collapsed;
            this.ReviewsList.ItemsSource = this.regComments;

        }


        private async void RemoveReview(object sender, RoutedEventArgs e)
        {

            Button btn = sender as Button;
            RegisteredComments regComment = btn.Tag as RegisteredComments;
            Review review = new Review()
            {
                RegisteredID = regComment.RegisteredID,
                Rate = regComment.Rate,
                BookID = regComment.BookID,
                Comment = regComment.Comment,
                ReviewID = regComment.ReviewID
            };
            var confirm = MessageBox.Show(
               $"Are you sure you want to remove user's \"{this.reg.UserName}\" review?\nThis action cannot be undone.",
               "Confirm Delete",
               MessageBoxButton.YesNo,
               MessageBoxImage.Warning);
            if (confirm != MessageBoxResult.Yes) return;
            ApiClient<Review> client = new ApiClient<Review>();
            ApiResultModel<bool> success;
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5049;
            client.Path = "api/Admin/RemoveReview";
            success = await client.PostAsyncRet<Review, bool>((Review)review);
            if (!success.Data)
            {
                MessageBox.Show("The operation failed, the review didn't got deleted.", "Error message", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.regComments.Remove(regComment);
            
            this.ReviewsList.ItemsSource = null;
            this.ReviewsList.ItemsSource = this.regComments;
        }



        // ════════════════════════════════════════════════════════════
        //  LOAD USER INFO
        // ════════════════════════════════════════════════════════════
        //private void LoadUser(string username)
        //{



        //        // ── Populate hero ────────────────────────────────────


        //        // Initials
        //        TxtInitials.Text = this.reg.UserName.Length >= 2
        //            ? $"{char.ToUpper(this.reg.UserName[0])}{char.ToUpper(this.reg.UserName[1])}"
        //            : char.ToUpper(this.reg.UserName[0]).ToString();

        //        // Role badge
        //        if (RoleStyle.TryGetValue(role, out var rs))
        //        {
        //            RoleBadge.Background = new SolidColorBrush(
        //                (Color)ColorConverter.ConvertFromString(rs[0]));
        //            TxtRoleBadge.Text = rs[2];
        //        }
        //        else
        //        {
        //            RoleBadge.Background = new SolidColorBrush(Colors.Gray);
        //            TxtRoleBadge.Text = $"👤 {this.reg.Role}";
        //        }

        //        // Banned badge
        //        if (this.reg.IsBanned)
        //        {
        //            BannedBadge.Visibility = Visibility.Visible;
        //            BannedBadge.Background = new SolidColorBrush(
        //                (Color)ColorConverter.ConvertFromString("#dc2626"));
        //        }

        //        // Avatar image

        //        // ── Load reviews ─────────────────────────────────────

        //}

        // ════════════════════════════════════════════════════════════
        //  LOAD REVIEWS (reuses same open connection)
        // ════════════════════════════════════════════════════════════


        // ════════════════════════════════════════════════════════════
        //  AVATAR IMAGE
        // ════════════════════════════════════════════════════════════
        //private void TryLoadAvatar(string path)
        //{
        //    if (string.IsNullOrWhiteSpace(path) || path == "None") return;
        //    try
        //    {
        //        var bmp = new BitmapImage();
        //        bmp.BeginInit();
        //        bmp.CacheOption = BitmapCacheOption.OnLoad;
        //        if (path.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        //            bmp.UriSource = new Uri(path);
        //        else if (File.Exists(path))
        //            bmp.UriSource = new Uri(path, UriKind.Absolute);
        //        else return;
        //        bmp.EndInit();

        //        AvatarBrush.ImageSource = bmp;
        //        // Hide initials when image loads successfully
        //        TxtInitials.Visibility = Visibility.Collapsed;
        //    }
        //    catch { /* keep initials */ }
        //}

        private void Close_Click(object sender, RoutedEventArgs e) => Close();

    }
}
