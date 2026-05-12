using IIstudyWSClient;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        upcoming = 2,
        today = 4,
    }
    public partial class ViewEvents : UserControl
    {
        private ObservableCollection<EventDetail> Events { get; set; }
        private ObservableCollection<EventDetail> FilteredEvents { get; set; }

        private FilterMode FilterMode { get; set; } = FilterMode.all;
        private string SearchText { get; set; } = "";
        public ViewEvents()
        {
            InitializeComponent();


            Init_page();
        }



        private async void Init_page()
        {
            this.GetEvents();
        }


        private async void GetEvents()
        {
            try
            {
                ApiClient<List<EventDetail>> client = new ApiClient<List<EventDetail>>();

                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5049;
                client.Path = "api/Guest/GetEventsDetails";
                List<EventDetail> events = await client.GetAsync();
                this.Events = new ObservableCollection<EventDetail>(events);

                if (this.Events == null)
                {
                    this.Events = new ObservableCollection<EventDetail>();
                    MessageBox.Show("Failed in reciving the Events from web service", "Request Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
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
        private void ApplyFilters()
        {
            this.FilteredEvents = new ObservableCollection<EventDetail>(this.Events.Where(e =>
            {
                bool searchBar = e.Event_name.ToLower().Contains(this.SearchText.ToLower());
                return searchBar;
            }));

            this.EventsList.ItemsSource = this.FilteredEvents;
        }

        
    }
}
