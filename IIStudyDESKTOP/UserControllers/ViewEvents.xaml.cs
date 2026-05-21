using IIStudyDESKTOP.WindowsPages;
using IIstudyWSClient;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IIStudyDESKTOP.UserControllers
{
    /// <summary>
    /// Interaction logic for ViewEvents.xaml
    /// </summary>
    
    enum FilterMode
    {
        all = 0,
        past = 1,
        today = 2,
        upcoming = 3,
        
    }
    public partial class ViewEvents : UserControl
    {
        private EditEventWindow EditEventWindow { get; set; }
        private CreateEventWindow CreateEventWindow { get; set; }
        private ObservableCollection<EventDetail> Events { get; set; }

        private FilterMode FilterMode { get; set; } = FilterMode.all;
        private string SearchText { get; set; } = "";

        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;

        public ViewEvents()
        {
            InitializeComponent();
            this.DataContext = this;
            
            Init_page();


        }



        private async Task Init_page()
        {
            await this.GetEvents();
            this.UpdateStatistics();
        }

        private async void Refresh(object sender, RoutedEventArgs e)
        {
            await this.Init_page();
            this.ApplyFilters();
        }

        private async void UpdateStatistics()
        {
            this.TxtPast.Text = this.Events.Count(e => e.Date.Date < DateTime.Now.Date).ToString();
            this.TxtToday.Text = this.Events.Count(e => e.Date.Date == DateTime.Now.Date).ToString();
            this.TxtUpcoming.Text = this.Events.Count(e => e.Date.Date > DateTime.Now.Date).ToString();
            this.TxtTotalEvents.Text = this.Events.Count().ToString();

        }

        private async Task GetEvents()
        {
            try
            {
                ApiClient<List<EventDetail>> client = new ApiClient<List<EventDetail>>();

                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5049;
                client.Path = "api/Guest/GetEventsDetails";
                List<EventDetail> events = await client.GetAsync();

                if (events == null)
                {
                    this.Events = new ObservableCollection<EventDetail>();
                    MessageBox.Show("Failed in reciving the Events from web service", "Request Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }

                this.Events = new ObservableCollection<EventDetail>(events);

                this.EventsList.ItemsSource = null;
                this.EventsList.ItemsSource = this.Events;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Faild to connect to host.", "Network Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            this.SearchText = this.SearchBox.Text;
            this.ApplyFilters();
        }

        private void SelectChipFilter(object sender, MouseButtonEventArgs e)
        {
            Border btn = sender as Border;
            this.FilterMode = (FilterMode)int.Parse(btn.Tag.ToString());
            this.SetActiveChip(btn);
            this.ApplyFilters();
            
        }

        private void ApplyFilters()
        {
            ObservableCollection<EventDetail> events;
            switch (this.FilterMode)
            {
                
                case FilterMode.upcoming:
                    events =new  ObservableCollection<EventDetail>(this.Events.Where(e => e.Date >= DateTime.Now.Date));
                    break;
                case FilterMode.past:
                    events = new ObservableCollection<EventDetail>(this.Events.Where(e => e.Date.Date <= DateTime.Now.Date));
                    break;
                case FilterMode.today:
                    events = new ObservableCollection<EventDetail>(this.Events.Where(e => e.Date.Date == DateTime.Now.Date));
                    break;
                default:
                    events = this.Events;
                    break;

            }

            ObservableCollection<EventDetail> filtered = new ObservableCollection<EventDetail>(events.Where(e =>
            {
                bool date = true;
                if (this.FromDate != null && this.ToDate != null)
                    date = e.Date.Date >= this.FromDate.Value.Date && e.Date.Date <= this.ToDate.Value.Date;
                else if (this.FromDate != null)
                    date = e.Date.Date >= this.FromDate.Value.Date;
                else if (this.ToDate != null)
                    date = e.Date.Date <= this.ToDate.Value.Date;

                bool searchBar = e.Event_name.ToLower().Contains(this.SearchText.ToLower());
                return searchBar && date;
            }));
            this.EventsList.ItemsSource = null;
            this.EventsList.ItemsSource = filtered;
        }

        private void SetActiveChip(Border active)
        {
            ChipAll.Opacity = 0.45;
            ChipUpcoming.Opacity = 0.45;
            ChipToday.Opacity = 0.45;

            ChipAllTxt.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#14b8a6"));
            ChipAll.Background = new SolidColorBrush(Colors.Transparent);
            ChipUpcoming.Background = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#eef2ff"));
            ChipToday.Background = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#fffbeb"));
            ChipPast.Background = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#f1f5f9"));
            active.Opacity = 1.0;

            if (active == ChipAll)
            {
                ChipAll.Background = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#14b8a6"));
                ChipAllTxt.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
                //ChipAll.Background = new LinearGradientBrush(
                //    (Color)ColorConverter.ConvertFromString("#042f2e"),
                //    (Color)ColorConverter.ConvertFromString("#14b8a6"),
                //    new Point(0, 0.5), new Point(1, 0.5));
            }
            else if (active == ChipUpcoming)
            {
                ChipUpcoming.Background = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#c7d2fe"));
            }
            else if (active == ChipToday)
            {
                ChipToday.Background = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#fde68a"));
            }
            else if (active == ChipPast)
            {
                ChipPast.Background = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#cbd5e1"));
            }
        }

        private void UpdateEvent(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            EventDetail Event = btn.Tag as EventDetail;

            var json = JsonSerializer.Serialize(Event);
            var copy = JsonSerializer.Deserialize<EventDetail>(json);

            this.EditEventWindow = new EditEventWindow(Event);
            bool? result = this.EditEventWindow.ShowDialog();
            if(result != true)
            {
                const string format = "yyyy-MM-dd";

                Event.Date = DateTime.ParseExact(
                                        Event.Date_event,
                                        format,
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None
                                    ); 
                Event.Date_event = copy.Date_event;
                Event.Event_name = copy.Event_name;
                Event.Details = copy.Details;
                this.EventsList.Items.Refresh();

            }

        }

        private async void DeleteEvent(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null || btn.Tag == null) return;
            Event Event = btn.Tag as Event;

            try
            {
                ApiClient<bool> client = new ApiClient<bool>();

                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5049;
                client.Path = "api/Admin/DeleteEvent";
                ApiResultModel<bool> response = await client.PostAsyncRet<Event,bool>(Event);

                if (response == null || !response.Success || !response.Data)
                {
                    MessageBox.Show("Failed in deleting Event from the data base", "Request Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;

                }
                this.Events.Remove((EventDetail)Event);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Faild to connect to host.", "Network Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ApplyFilters();
        }
        private void CreateEvent(object sender, RoutedEventArgs e)
        {
            this.CreateEventWindow = new CreateEventWindow();
            this.CreateEventWindow.ShowDialog();
        }
    }
}
